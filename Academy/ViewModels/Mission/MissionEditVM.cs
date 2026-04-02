using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Mission
{
    public class MissionEditVM
    {
        public int Id { get; set; }

        public string Title { get; set; }


        public string Description { get; set; }

        public string ExistingImage { get; set; } 
        public IFormFile? Image { get; set; }
    }
}
