using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Feature
{
    public class FeatureEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Təsvir mütləqdir.")]
        [StringLength(1000, MinimumLength = 5)]
        public string Description { get; set; } = null!;
    }
}
