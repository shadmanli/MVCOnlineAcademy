using Academy.ViewModels.ContactItem;

namespace Academy.ViewModels.ContactSection
{
    public class ContactSectionVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }

        public IEnumerable<ContactItemVM> Items { get; set; }
    }
}
