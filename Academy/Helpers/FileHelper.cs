namespace Academy.Helpers
{
    public static class FileHelper
    {
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private static readonly string[] AllowedVideoExtensions = { ".mp4", ".webm", ".mov", ".avi" };
        private const long MaxImageSize = 5 * 1024 * 1024;    // 5 MB
        private const long MaxVideoSize = 500 * 1024 * 1024;  // 500 MB

        public static void ValidateImage(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file), "Şəkil faylı boş ola bilməz.");
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedImageExtensions.Contains(ext))
                throw new InvalidOperationException($"İcazəsiz şəkil formatı: {ext}. İcazə verilənlər: {string.Join(", ", AllowedImageExtensions)}");
            if (file.Length > MaxImageSize)
                throw new InvalidOperationException($"Şəkil həcmi 5MB-dan böyük ola bilməz. Cari həcm: {file.Length / 1024 / 1024}MB");
        }

        public static void ValidateVideo(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file), "Video faylı boş ola bilməz.");
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedVideoExtensions.Contains(ext))
                throw new InvalidOperationException($"İcazəsiz video formatı: {ext}. İcazə verilənlər: {string.Join(", ", AllowedVideoExtensions)}");
            if (file.Length > MaxVideoSize)
                throw new InvalidOperationException($"Video həcmi 500MB-dan böyük ola bilməz.");
        }

        public static async Task<string> SaveFileAsync(IFormFile file, string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Yalnız extension saxla — orijinal fayl adını istifadə etmə (path traversal qarşısı)
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var safeFileName = Guid.NewGuid().ToString("N") + ext;
            var fullPath = Path.Combine(folderPath, safeFileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return safeFileName;
        }

        public static void DeleteFile(string folderPath, string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            var fullPath = Path.Combine(folderPath, fileName);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
