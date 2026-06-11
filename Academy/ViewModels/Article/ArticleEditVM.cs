using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.Article
{
    public class ArticleEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Qısa təsvir mütləqdir.")]
        [StringLength(500, MinimumLength = 5)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Mətn mütləqdir.")]
        [StringLength(10000, MinimumLength = 10)]
        public string Text { get; set; } = null!;

        public IFormFile? Image { get; set; }
        public string? ExistingImage { get; set; }

        [Required(ErrorMessage = "Mövzu seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mövzu seçilməlidir.")]
        public int TopicId { get; set; }

        public List<SelectListItem>? Topics { get; set; }
    }
}
