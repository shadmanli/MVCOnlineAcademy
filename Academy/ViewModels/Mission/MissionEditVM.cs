using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Mission
{
    public class MissionEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Təsvir mütləqdir.")]
        [StringLength(2000, MinimumLength = 5)]
        public string Description { get; set; } = null!;

        public string? ExistingImage { get; set; }
        public IFormFile? Image { get; set; }
    }
}
