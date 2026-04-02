using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.ContactItem
{
    public class ContactItemEditVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int ContactSectionId { get; set; }
        public List<SelectListItem> Sections { get; set; }
    }
}
