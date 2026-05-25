using System;

namespace Academy.Models
{
    public enum AttendeeStatus
    {
        Waiting,  // Waiting Room-dad?r
        Admitted, // Mü?llim q?bul etdi / Avto q?bul olundu
        Removed,  // Mü?llim t?r?find?n ç?xar?ld? (Kick)
        Left      // D?rsi özü t?rk etdi
    }

    public class LiveClassAttendance : BaseEntity
    {
        public int LiveClassId { get; set; }
        public LiveClass LiveClass { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // 1.4 Waiting room statusu
        public AttendeeStatus Status { get; set; } = AttendeeStatus.Waiting;

        public string DeviceInfo { get; set; } // H?m cihaz, h?m IP track etm?k üçün

        // 1.9 Attendance Tracking
        public DateTime? FirstJoinTime { get; set; }
        public DateTime? LastLeaveTime { get; set; }
        public int TotalWatchedMinutes { get; set; } // D?rsd? qald??? real müdd?t
    }
}
