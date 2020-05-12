using Portfolio.Shared.DataModels;
using Portfolio.Shared.Interfaces;
using System.Collections.Generic;

namespace Portfolio.Shared.PageModels
{
    public class HomePageModel : IPageModel
    {
        public int HeroId { get; set; }
        public string TopContentTitle { get; set; }
        public FlexComponentReference TopContent { get; set; }
        public string MiddleContentTitle { get; set; }
        public FlexComponentReference MiddleContentLeft { get; set; }
        public FlexComponentReference MiddleContentRight { get; set; }
        public string BottomContentTitle { get; set; }
        public FlexComponentReference BottomContent { get; set; }
        public bool ShowBlogPosts { get; set; }
        public string FooterContentTitle { get; set; }
        public FlexComponentReference FooterTopContent { get; set; }
        public List<Link> Links => new List<Link>
        {
            new Link { Name = TopContent.FlexName, Reference = TopContent.FlexId.ToString(), FragmentRoute = true },
            new Link { Name = MiddleContentLeft.FlexName, Reference = MiddleContentLeft.FlexId.ToString(), FragmentRoute = true  },
            new Link { Name = MiddleContentRight.FlexName, Reference = MiddleContentRight.FlexId.ToString(), FragmentRoute = true  },
            new Link { Name = BottomContent.FlexName, Reference = BottomContent.FlexId.ToString(), FragmentRoute = true  },
            new Link { Name = FooterTopContent.FlexName, Reference = FooterTopContent.FlexId.ToString(), FragmentRoute = true  },
        };
    }
}
