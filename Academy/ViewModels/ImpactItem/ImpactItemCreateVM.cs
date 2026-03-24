using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.ImpactItem
{


    public class ImpactItemCreateVM
    {
        public string Title { get; set; }       
        public string Description { get; set; }   

        public int? ImpactSectionId { get; set; }  

        public IFormFile Image { get; set; }

        public List<SelectListItem> Sections { get; set; }
    }
}
