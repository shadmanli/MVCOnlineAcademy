using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.ImpactItem
{
    public class ImpactItemEditVM
    {
        public int Id { get; set; }

        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Təsvir mütləqdir.")]
        [StringLength(1000, MinimumLength = 5)]
        public string Description { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Impact bölməsi seçilməlidir.")]
        public int? ImpactSectionId { get; set; }

        public List<SelectListItem>? Sections { get; set; }
    }
}
