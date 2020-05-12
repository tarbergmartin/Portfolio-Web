using Portfolio.Shared.Interfaces;

namespace Portfolio.Shared.ComponentModels
{
    public class AboutModel : IComponentModel
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string HtmlText { get; set; }
    }
}
