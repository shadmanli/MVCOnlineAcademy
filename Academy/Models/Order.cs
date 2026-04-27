using System;
using System.Collections.Generic;

namespace Academy.Models
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}