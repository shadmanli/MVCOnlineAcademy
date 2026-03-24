namespace Academy.Models
{
    public class ContactItem:BaseEntity
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public int ContactSectionId { get; set; }
        public ContactSection ContactSection { get; set; }
    }
}
