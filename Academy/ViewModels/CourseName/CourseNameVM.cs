namespace Academy.ViewModels.CourseName
{
    public class CourseNameVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<string> CategoryNames { get; set; } = new();
        public List<int> CategoryIds { get; set; } = new();
    }

    public class CourseNameCreateVM
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public List<int> CategoryIds { get; set; } = new();
    }

    public class CourseNameEditVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<int> CategoryIds { get; set; } = new();
    }

    // AJAX response DTO
    public class CourseNameCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
