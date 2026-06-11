using Academy.ViewModels.Course;
using FluentValidation;

namespace Academy.Validators
{
    public class CourseCreateValidator : AbstractValidator<CourseCreateVM>
    {
        private static readonly string[] AllowedImageExts = { ".jpg", ".jpeg", ".png", ".webp" };
        private static readonly string[] AllowedVideoExts = { ".mp4", ".mov", ".avi" };
        private const long MaxImageBytes = 5 * 1024 * 1024;    // 5 MB
        private const long MaxVideoBytes = 500 * 1024 * 1024;  // 500 MB

        public CourseCreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Kurs adı daxil edilməlidir.")
                .NotNull().WithMessage("Kurs adı daxil edilməlidir.")
                .MinimumLength(3).WithMessage("Kurs adı minimum 3 simvol olmalıdır.")
                .MaximumLength(200).WithMessage("Kurs adı maksimum 200 simvol ola bilər.")
                .Must(v => !string.IsNullOrWhiteSpace(v)).WithMessage("Kurs adı yalnız boşluqdan ibarət ola bilməz.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Kurs təsviri daxil edilməlidir.")
                .MinimumLength(10).WithMessage("Kurs təsviri minimum 10 simvol olmalıdır.")
                .MaximumLength(5000).WithMessage("Kurs təsviri maksimum 5000 simvol ola bilər.")
                .Must(v => !string.IsNullOrWhiteSpace(v)).WithMessage("Kurs təsviri yalnız boşluqdan ibarət ola bilməz.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Qiymət daxil edilməlidir.")
                .GreaterThanOrEqualTo(0).WithMessage("Qiymət mənfi ola bilməz.")
                .LessThanOrEqualTo(10000).WithMessage("Qiymət maksimum 10000 ola bilər.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Kurs şəkli seçilməlidir.")
                .Must(f => f != null && f.Length > 0).WithMessage("Kurs şəkli seçilməlidir.")
                .Must(f => f == null || AllowedImageExts.Contains(
                    Path.GetExtension(f.FileName).ToLowerInvariant()))
                    .WithMessage("Şəkil formatı düzgün deyil. İcazə verilənlər: jpg, jpeg, png, webp.")
                .Must(f => f == null || f.Length <= MaxImageBytes)
                    .WithMessage("Şəkil ölçüsü maksimum 5 MB ola bilər.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Kateqoriya seçilməlidir.");

            RuleFor(x => x.LanguageId)
                .GreaterThan(0).WithMessage("Dil seçilməlidir.");

            RuleFor(x => x.InstructorId)
                .GreaterThan(0).WithMessage("Müəllim seçilməlidir.");

            RuleFor(x => x.Level)
                .NotEmpty().WithMessage("Səviyyə seçilməlidir.")
                .Must(l => new[] { "Beginner", "Intermediate", "Advanced" }.Contains(l))
                .WithMessage("Səviyyə düzgün deyil.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Müddət 0-dan böyük olmalıdır.")
                .LessThanOrEqualTo(10000).WithMessage("Müddət maksimum 10000 dəqiqə ola bilər.");

            // Video validation (əgər video əlavə edilibsə)
            RuleForEach(x => x.VideoTitles)
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Video başlığı daxil edilməlidir.");

            RuleForEach(x => x.VideoFiles)
                .Must(f => f == null || AllowedVideoExts.Contains(
                    Path.GetExtension(f.FileName).ToLowerInvariant()))
                    .WithMessage("Video formatı düzgün deyil. İcazə verilənlər: mp4, mov, avi.")
                .Must(f => f == null || f.Length <= MaxVideoBytes)
                    .WithMessage("Video ölçüsü maksimum 500 MB ola bilər.");
        }
    }
}
