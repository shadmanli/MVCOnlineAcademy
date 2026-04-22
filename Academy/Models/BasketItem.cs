namespace Academy.Models
{
    public class BasketItem : BaseEntity
    {
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
