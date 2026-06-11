using Microsoft.AspNetCore.SignalR;
using Academy.Data;
using Academy.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Academy.Hubs
{
    public class LiveClassHub : Hub
    {
        private readonly AppDbContext _context;
        // Memory store for participant states in rooms (roomId -> ParticipantId -> ParticipantState)
        private static ConcurrentDictionary<string, ConcurrentDictionary<string, Participant>> _roomParticipants 
            = new ConcurrentDictionary<string, ConcurrentDictionary<string, Participant>>();

        public LiveClassHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task JoinRoom(string roomId, string userId, string fullName, bool isTeacher)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            var room = _roomParticipants.GetOrAdd(roomId, new ConcurrentDictionary<string, Participant>());

            var participant = new Participant 
            { 
                ConnectionId = Context.ConnectionId, 
                UserId = userId,
                FullName = fullName, 
                IsTeacher = isTeacher,
                IsMicOn = false,
                IsVideoOn = false,
                HandRaised = false
            };

            // Remove old connection if same user rejoins (simple handling)
            var oldUserKeys = room.Where(p => p.Value.UserId == userId).Select(k => k.Key).ToList();
            foreach (var key in oldUserKeys) 
            {
                if (room.TryRemove(key, out var oldParticipant))
                {
                    await Clients.Group(roomId).SendAsync("ParticipantLeft", key);
                }
            }

            room.TryAdd(Context.ConnectionId, participant);

            // Broadcast to the room that a new participant joined
            await Clients.Group(roomId).SendAsync("ParticipantJoined", participant);

            // Send current list of participants to the newly joined user
            await Clients.Caller.SendAsync("UpdateParticipants", room.Values.ToList());
        }

        public async Task LeaveRoom(string roomId)
        {
            if (_roomParticipants.TryGetValue(roomId, out var room))
            {
                if (room.TryRemove(Context.ConnectionId, out _))
                {
                    await Clients.Group(roomId).SendAsync("ParticipantLeft", Context.ConnectionId);
                }
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task StartClass(string roomId)
        {
            if (!_roomParticipants.TryGetValue(roomId, out var room) ||
                !room.TryGetValue(Context.ConnectionId, out var participant) ||
                !participant.IsTeacher)
            {
                return; // Only host can start
            }

            var liveClass = await _context.LiveClasses.FirstOrDefaultAsync(l => l.RoomId == roomId);
            if(liveClass != null && liveClass.Status != LiveSessionStatus.Live && liveClass.TeacherId == participant.UserId)
            {
                liveClass.Status = LiveSessionStatus.Live;
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ClassStatusChanged", roomId, "Live");
            }
        }

        public async Task EndClass(string roomId)
        {
            if (!_roomParticipants.TryGetValue(roomId, out var room) ||
                !room.TryGetValue(Context.ConnectionId, out var participant) ||
                !participant.IsTeacher)
            {
                return; // Only host can end
            }

            var liveClass = await _context.LiveClasses.FirstOrDefaultAsync(l => l.RoomId == roomId);
            if(liveClass != null && liveClass.TeacherId == participant.UserId)
            {
                liveClass.Status = LiveSessionStatus.Ended;
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ClassStatusChanged", roomId, "Ended");
            }
        }
        
        public async Task RemoveParticipant(string roomId, string targetConnectionId)
        {
            if (!_roomParticipants.TryGetValue(roomId, out var room) ||
                !room.TryGetValue(Context.ConnectionId, out var participant) ||
                !participant.IsTeacher)
            {
                return; // Only host can remove
            }
            
            if (room.TryRemove(targetConnectionId, out _))
            {
                await Clients.Client(targetConnectionId).SendAsync("KickedOut");
                await Clients.Group(roomId).SendAsync("ParticipantLeft", targetConnectionId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var room in _roomParticipants)
            {
                if (room.Value.TryRemove(Context.ConnectionId, out _))
                {
                    await Clients.Group(room.Key).SendAsync("ParticipantLeft", Context.ConnectionId);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendSignal(string targetConnectionId, string signal)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveSignal", Context.ConnectionId, signal);
        }

        // ── WebRTC Signaling ─────────────────────────────────────────
        // Peer A → B: "offer" SDP göndərir
        public async Task SendOffer(string targetConnectionId, string sdp)
        {
            await Clients.Client(targetConnectionId).SendAsync(
                "ReceiveOffer", Context.ConnectionId, sdp);
        }

        // Peer B → A: "answer" SDP göndərir
        public async Task SendAnswer(string targetConnectionId, string sdp)
        {
            await Clients.Client(targetConnectionId).SendAsync(
                "ReceiveAnswer", Context.ConnectionId, sdp);
        }

        // ICE candidate mübadiləsi
        public async Task SendIceCandidate(string targetConnectionId, string candidate)
        {
            await Clients.Client(targetConnectionId).SendAsync(
                "ReceiveIceCandidate", Context.ConnectionId, candidate);
        }

        // Ekran paylaşması başladı/bitdi
        public async Task ToggleScreenShare(string roomId, bool isSharing)
        {
            if (_roomParticipants.TryGetValue(roomId, out var room) &&
                room.TryGetValue(Context.ConnectionId, out var p))
            {
                await Clients.Group(roomId).SendAsync(
                    "ScreenShareToggled", Context.ConnectionId, p.FullName, isSharing);
            }
        }

        public async Task ToggleMic(string roomId, bool state)
        {
            if (_roomParticipants.TryGetValue(roomId, out var room) && room.TryGetValue(Context.ConnectionId, out var p))
            {
                p.IsMicOn = state;
                await Clients.Group(roomId).SendAsync("MicToggled", Context.ConnectionId, state);
            }
        }

        public async Task ToggleVideo(string roomId, bool state)
        {
            if (_roomParticipants.TryGetValue(roomId, out var room) && room.TryGetValue(Context.ConnectionId, out var p))
            {
                p.IsVideoOn = state;
                await Clients.Group(roomId).SendAsync("VideoToggled", Context.ConnectionId, state);
            }
        }

        public async Task RaiseHand(string roomId)
        {
            if (_roomParticipants.TryGetValue(roomId, out var room) && room.TryGetValue(Context.ConnectionId, out var p))
            {
                p.HandRaised = !p.HandRaised;
                await Clients.Group(roomId).SendAsync("HandRaised", Context.ConnectionId, p.HandRaised);
            }
        }

        public async Task LowerHand(string roomId, string targetConnectionId)
        {
            if (_roomParticipants.TryGetValue(roomId, out var room) && 
                room.TryGetValue(Context.ConnectionId, out var caller) && caller.IsTeacher &&
                room.TryGetValue(targetConnectionId, out var target))
            {
                target.HandRaised = false;
                await Clients.Group(roomId).SendAsync("HandRaised", targetConnectionId, false);
            }
        }

        public async Task SendMessage(string roomId, string message, string fullName, bool isTeacher)
        {
            var msgId = Guid.NewGuid().ToString("N");
            await Clients.Group(roomId).SendAsync("ReceiveMessage", msgId, fullName, message, DateTime.Now.ToString("HH:mm"), isTeacher);
        }

        public async Task UserTyping(string roomId, string fullName, bool isTyping)
        {
            await Clients.Group(roomId).SendAsync("UserTypingStatus", Context.ConnectionId, fullName, isTyping);
        }
    }

    public class Participant
    {
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsMicOn { get; set; }
        public bool IsVideoOn { get; set; }
        public bool HandRaised { get; set; }
    }
}
