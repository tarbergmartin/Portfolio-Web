using Microsoft.SharePoint.Client;
using System.Linq;

namespace Portfolio.API.Classes
{
    public static class Queries
    {
        public static IQueryable<ListItem> QueryHomePages(this ListItemCollection pages)
        {
            return pages.Where(page => page.ContentType.Name == "HomePage")
                        .Include(page => page["PageName"],
                                 page => page["Hero"],
                                 page => page["TopContentTitle"],
                                 page => page["TopContent"],
                                 page => page["MiddleContentTitle"],
                                 page => page["MiddleContentLeft"],
                                 page => page["MiddleContentRight"],
                                 page => page["BottomContentTitle"],
                                 page => page["BottomContent"],
                                 page => page["ShowBlogPosts"],
                                 page => page["FooterContentTitle"],
                                 page => page["FooterTopContent"]);
        }

        public static IQueryable<ListItem> QueryBlogPosts(this ListItemCollection pages)
        {
            return pages.Where(page => page.ContentType.Name == "BlogPage")
                        .Include(page => page.Id,
                                 page => page["Hero"],
                                 page => page["PublishedDate1"],
                                 page => page["Text"],
                                 page => page["PageName"]);
        }

        public static IQueryable<ListItem> QueryBlogPagesById(this ListItemCollection pages, int id)
        {
            return pages.Where(page => page.ContentType.Name == "BlogPage" && page.Id == id)
                        .Include(page => page.Id,
                                 page => page["PageName"],
                                 page => page["Hero"],
                                 page => page["PublishedDate1"],
                                 page => page["Text"],
                                 page => page["Title"]);
        }

        public static IQueryable<ListItem> QueryByFlexibleListItemId(this ListItemCollection collection, int id)
        {
            return collection.Where(component => component.Id == id)
                             .IncludeWithDefaultProperties(component => component.ContentType);
        }

        public static IQueryable<ListItem> QueryByHeroId(this ListItemCollection collection, int id)
        {
            return collection.Where(component => component.Id == id)
                             .Include(component => component.Id,
                                      component => component["ComponentName"],
                                      component => component["Text"]);
        }

        public static IQueryable<ListItem> QueryHighlights(this ListItemCollection collection)
        {
            return collection.Include(highlight => highlight.Id,
                                      highlight => highlight["Title"],
                                      highlight => highlight["Text"]);
        }

        public static IQueryable<ListItem> QuerySkills(this ListItemCollection collection)
        {
            return collection.Include(highlight => highlight.Id,
                                      highlight => highlight["SkillName"],
                                      highlight => highlight["SkillLevel"]);
        }

        public static IQueryable<ListItem> QueryProjects(this ListItemCollection collection)
        {
            return collection.Include(project => project.Id,
                                      project => project["Title"],
                                      project => project["Technology"],
                                      project => project["ProjectLink"],
                                      project => project["ProjectLinkText"]);
        }
    }
}
