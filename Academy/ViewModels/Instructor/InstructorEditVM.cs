using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Instructor
{
    public class InstructorEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad mütləqdir.")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = null!;
    }
}
