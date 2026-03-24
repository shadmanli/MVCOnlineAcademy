namespace Academy.ViewModels.ContactItem
{
    public class ContactItemVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public int ContactSectionId { get; set; }
        public string SectionName { get; set; }
    }
}
