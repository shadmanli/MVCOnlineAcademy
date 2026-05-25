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

            // Send system message to chat
            await Clients.Group(roomId).SendAsync("ReceiveSystemMessage", $"{fullName} ota?a qo?uldu", DateTime.Now.ToString("HH:mm"));

            // Send current participants to the newly joined user
            await Clients.Caller.SendAsync("UpdateParticipants", room.Values.ToList());
        }

        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

            if (_roomParticipants.TryGetValue(roomId, out var room))
            {
                if (room.TryRemove(Context.ConnectionId, out var participant))
                {
                    await Clients.Group(roomId).SendAsync("ParticipantLeft", Context.ConnectionId);
                    await Clients.Group(roomId).SendAsync("ReceiveSystemMessage", $"{participant.FullName} otaqdan ayr?ld?", DateTime.Now.ToString("HH:mm"));
                }
            }
        }

        public async Task StartClass(string roomId)
        {
            var liveClass = await _context.LiveClasses.FirstOrDefaultAsync(l => l.RoomId == roomId);
            if (liveClass != null && liveClass.Status != LiveSessionStatus.Live)
            {
                liveClass.Status = LiveSessionStatus.Live;
                await _context.SaveChangesAsync();
                await Clients.Group(roomId).SendAsync("ClassStarted");
            }
        }

        public async Task EndClass(string roomId)
        {
            var liveClass = await _context.LiveClasses.FirstOrDefaultAsync(l => l.RoomId == roomId);
            if (liveClass != null && liveClass.Status == LiveSessionStatus.Live)
            {
                liveClass.Status = LiveSessionStatus.Ended;
                await _context.SaveChangesAsync();
                await Clients.Group(roomId).SendAsync("ClassEnded");
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

        public async Task SendMessage(string roomId, string message, string fullName, bool isTeacher)
        {
            var msgId = Guid.NewGuid().ToString("N");
            await Clients.Group(roomId).SendAsync("ReceiveMessage", msgId, fullName, message, DateTime.Now.ToString("HH:mm"), isTeacher);
        }

        public async Task UserTyping(string roomId, string fullName, bool isTyping)
        {
            await Clients.Group(roomId).SendAsync("UserTypingStatus", Context.ConnectionId, fullName, isTyping);
        }

        // WebRTC Signaling
        public async Task SendSignal(string targetConnectionId, string signal)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveSignal", Context.ConnectionId, signal);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var roomKey in _roomParticipants.Keys)
            {
                if (_roomParticipants.TryGetValue(roomKey, out var room))
                {
                    if (room.TryRemove(Context.ConnectionId, out var participant))
                    {
                        await Clients.Group(roomKey).SendAsync("ParticipantLeft", Context.ConnectionId);
                        await Clients.Group(roomKey).SendAsync("ReceiveSystemMessage", $"{participant.FullName} otaqdan ayr?ld?", DateTime.Now.ToString("HH:mm"));
                    }
                }
            }
            await base.OnDisconnectedAsync(exception);
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
