namespace Academy.Models
{
    public class ContactSection:BaseEntity
    {
        public string Title { get; set; }
        public string SubTitle  { get; set; }
         public List<ContactItem> ContactItems { get; set; }
    }
}
