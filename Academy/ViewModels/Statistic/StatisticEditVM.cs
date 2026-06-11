using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Statistic
{
    public class StatisticEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Say mənfi ola bilməz.")]
        public int Count { get; set; }
    }
}
