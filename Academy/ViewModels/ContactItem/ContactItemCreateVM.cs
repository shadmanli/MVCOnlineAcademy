using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.ContactItem
{
    public class ContactItemCreateVM
    {
        [Required(ErrorMessage = "Təsvir mütləqdir.")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Təsvir 2-500 simvol arasında olmalıdır.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Ünvan mütləqdir.")]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "Ünvan 2-300 simvol arasında olmalıdır.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Əlaqə bölməsi seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Əlaqə bölməsi seçilməlidir.")]
        public int ContactSectionId { get; set; }

        public List<SelectListItem>? Sections { get; set; }
    }
}
