using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Statistic
{
    public class StatisticCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlıq 2-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Say mənfi ola bilməz.")]
        public int Count { get; set; }
    }
}
