using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.Article
{
    public class ArticleCreateVM
    {
        [Required(ErrorMessage = "Şəkil mütləqdir.")]
        public IFormFile Image { get; set; } = null!;

        [Required(ErrorMessage = "Qısa təsvir mütləqdir.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Qısa təsvir 5-500 simvol arasında olmalıdır.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Mətn mütləqdir.")]
        [StringLength(10000, MinimumLength = 10, ErrorMessage = "Mətn minimum 10 simvol olmalıdır.")]
        public string Text { get; set; } = null!;

        [Required(ErrorMessage = "Mövzu seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mövzu seçilməlidir.")]
        public int TopicId { get; set; }

        public List<SelectListItem>? Topics { get; set; }
    }
}
