using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.ImpactItem
{
    public class ImpactItemCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlıq 2-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Təsvir mütləqdir.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Təsvir 5-1000 simvol arasında olmalıdır.")]
        public string Description { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Impact bölməsi seçilməlidir.")]
        public int? ImpactSectionId { get; set; }

        [Required(ErrorMessage = "Şəkil mütləqdir.")]
        public IFormFile Image { get; set; } = null!;

        public List<SelectListItem>? Sections { get; set; }
    }
}
