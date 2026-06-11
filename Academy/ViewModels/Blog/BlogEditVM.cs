using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Blog
{
    public class BlogEditVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Məzmun mütləqdir.")]
        [StringLength(5000, MinimumLength = 10)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Ad mütləqdir.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        public IFormFile? Image { get; set; }
        public string? ExistingImage { get; set; }
    }
}
