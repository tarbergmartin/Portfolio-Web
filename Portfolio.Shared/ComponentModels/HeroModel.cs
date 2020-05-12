using Portfolio.Shared.Interfaces;

namespace Portfolio.Shared.ComponentModels
{
    public class HeroModel : IComponentModel
    {
        public string Image { get; set; }
        public string HtmlText { get; set; }
    }
}
