using System.Diagnostics.Contracts;

namespace Academy.Models
{
    public class Statistic:BaseEntity
    {
        public string Title { get; set; }
        public int Count { get; set; }
    }
}
