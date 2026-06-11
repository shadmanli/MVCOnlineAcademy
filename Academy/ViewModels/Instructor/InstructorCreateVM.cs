using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Instructor
{
    public class InstructorCreateVM
    {
        [Required(ErrorMessage = "Ad Soyad mütləqdir.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ad Soyad 2-100 simvol arasında olmalıdır.")]
        public string FullName { get; set; } = null!;
    }
}
