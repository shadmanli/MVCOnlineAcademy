using Academy.ViewModels.ImpactItem;

namespace Academy.ViewModels.ImpactSection
{
    public class ImpactSectionVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public IEnumerable<ImpactItemVM> Items { get; set; } 
    }
}
