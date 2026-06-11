using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Blog
{
    public class BlogCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Başlıq 3-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Məzmun mütləqdir.")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Məzmun minimum 10 simvol olmalıdır.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Şəkil mütləqdir.")]
        public IFormFile Image { get; set; } = null!;

        [Required(ErrorMessage = "Ad mütləqdir.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
