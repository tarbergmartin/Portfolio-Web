using Portfolio.Shared.Interfaces;

namespace Portfolio.Shared.ComponentModels
{
    public class ContactModel : IComponentModel
    {
        public string SendMailTo { get; set; }
        public string HtmlText { get; set; }
    }
}
