using Portfolio.Shared.Interfaces;
using System;

namespace Portfolio.Shared.PageModels
{
    public class BlogPageModel : IPageModel
    {
        public string Title { get; set; }
        public int HeroId { get; set; }
        public DateTime PublishedDate { get; set; }
        public string HtmlText { get; set; }
    }
}
