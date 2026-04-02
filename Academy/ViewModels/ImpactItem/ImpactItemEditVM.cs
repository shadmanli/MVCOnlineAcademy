using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.ImpactItem
{
    public class ImpactItemEditVM
    {
        public int Id { get; set; }

        public string Image { get; set; } // köhnə şəkil

        public IFormFile ImageFile { get; set; } // yeni upload

        public string Title { get; set; }
        public string Description { get; set; }

        public int? ImpactSectionId { get; set; }
        public List<SelectListItem> Sections { get; set; }

    }
}
