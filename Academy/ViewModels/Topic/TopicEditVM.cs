using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Topic
{
    public class TopicEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [StringLength(300)]
        public string? SubTitle { get; set; }
    }
}
