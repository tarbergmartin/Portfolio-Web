using Portfolio.Shared.DataModels;
using Portfolio.Shared.Interfaces;
using System.Collections.Generic;

namespace Portfolio.Shared.ComponentModels
{
    public class BlogModel : IComponentModel
    {
        public List<BlogPost> BlogPosts { get; set; }
    }
}
