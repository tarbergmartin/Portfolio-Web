using System;

namespace Portfolio.Shared.DataModels
{
    public class BlogPost
    {
        public int BlogPageId { get; set; }
        public string Image { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Title { get; set; }
        public string Teaser { get; set; }
    }
}
