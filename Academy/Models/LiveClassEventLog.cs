using System;

namespace Academy.Models
{
    // 1.5 Late Join & Timeline Tracking üçün
    public class LiveClassEventLog : BaseEntity
    {
        public int LiveClassId { get; set; }
        public LiveClass LiveClass { get; set; }

        public string EventType { get; set; } // Started, Paused, Resumed, Locked, Ended_Forcefully
        public string EventData { get; set; } // JSON format?nda ?lav? detallar (m?s?l?n: kim pause etdi)
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
