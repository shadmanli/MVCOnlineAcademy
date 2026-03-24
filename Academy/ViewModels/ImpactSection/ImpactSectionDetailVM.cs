using Academy.ViewModels.ImpactItem;

namespace Academy.ViewModels.ImpactSection
{
    public class ImpactSectionDetailVM
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string SubTitle { get; set; }

        public List<ImpactItemVM> Items { get; set; }
    }
}
