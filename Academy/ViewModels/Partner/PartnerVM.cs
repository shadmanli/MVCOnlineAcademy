namespace Academy.ViewModels.Partner
{
    public class PartnerVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }

    public class PartnerCreateVM
    {
        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Order { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }

    public class PartnerEditVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}
