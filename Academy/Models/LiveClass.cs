using System;
using System.Collections.Generic;

namespace Academy.Models
{
    public enum LiveSessionStatus
    {
        Scheduled,  // Planlan?b
        Live,       // Aktivdir
        Paused,     // Dayand?r?l?b
        Ended,      // Bitib
        Canceled    // L??v edilib
    }

    public class LiveClass : BaseEntity
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int? LessonId { get; set; }
        public Lesson? Lesson { get; set; }

        public int? InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        public string? TeacherId { get; set; }
        public AppUser Teacher { get; set; }

        public string Title { get; set; }
        public string Topic { get; set; }

        // 1.1 Unikal v? T?hl�k?sizlik
        public string RoomId { get; set; } = Guid.NewGuid().ToString("N");
        public string SecureToken { get; set; } // JWT v? ya Access Token ���n saxlanacaq

        // 1.2 Schedule
        public DateTime ScheduledDate { get; set; }
        public int DurationMinutes { get; set; }

        // 1.6 Session State
        public LiveSessionStatus Status { get; set; } = LiveSessionStatus.Scheduled;

        // 1.4 & 1.7 Control
        public bool AutoAdmitStudents { get; set; } = false; // Waiting room idar?si
        public bool IsLocked { get; set; } = false;          // Ota??n kilitl?nm?si

        public ICollection<LiveClassAttendance> Attendances { get; set; } = new List<LiveClassAttendance>();
        public ICollection<LiveClassEventLog> EventLogs { get; set; } = new List<LiveClassEventLog>();
    }
}
