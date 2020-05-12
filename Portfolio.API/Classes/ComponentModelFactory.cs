using Microsoft.SharePoint.Client;
using Portfolio.Shared.ComponentModels;
using Portfolio.Shared.DataModels;
using Portfolio.Shared.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.API.Classes
{
    public class ComponentModelFactory
    {
        public async Task<HeroModel> GetHeroModelAsync(ListItem listItem, ClientContext ctx)
        {
            if (listItem == null)
                return null;

            var base64Image = string.Empty;
            var attachments = ctx.LoadQuery(listItem.AttachmentFiles.Include(a => a.ServerRelativeUrl));

            await ctx.ExecuteQueryAsync();

            var image = attachments.FirstOrDefault();

            if (image != null)
            {
                base64Image = await ctx.GetImageAsBase64String(image.ServerRelativeUrl);
            }
         
            return new HeroModel
            {
                ComponentId = listItem.Id,
                ComponentName = listItem["ComponentName"] as string,
                Image = base64Image,
                HtmlText = (listItem["Text"] as string).GetHtmlContent()
            };
        }

        public HomePageModel GetHomePageModel(ListItem homePage)
        {
            if (homePage == null)
                return null;

            var hasShowBlogPosts = bool.TryParse(homePage["ShowBlogPosts"].ToString(), out bool showBlogPosts);

            return new HomePageModel
            {
                PageName = homePage["TopContentTitle"] as string,
                HeroId = (homePage["Hero"] as FieldLookupValue).GetLookupFieldValue(),
                TopContentTitle = homePage["TopContentTitle"] as string,
                TopContent = (homePage["TopContent"] as FieldLookupValue).GetFlexComponentReference(),
                MiddleContentTitle = homePage["MiddleContentTitle"] as string,
                MiddleContentLeft = (homePage["MiddleContentLeft"] as FieldLookupValue).GetFlexComponentReference(),
                MiddleContentRight = (homePage["MiddleContentRight"] as FieldLookupValue).GetFlexComponentReference(),
                BottomContentTitle = homePage["BottomContentTitle"] as string,
                BottomContent = (homePage["BottomContent"] as FieldLookupValue).GetFlexComponentReference(),
                ShowBlogPosts = hasShowBlogPosts ? showBlogPosts : false,
                FooterContentTitle = (homePage["FooterContentTitle"]) as string,
                FooterTopContent = (homePage["FooterTopContent"] as FieldLookupValue).GetFlexComponentReference()
            };
        }
        public BlogPageModel GetBlogPageModel(ListItem blogPage)
        {
            if (blogPage == null)
                return null;

            var hasDate = DateTime.TryParse(blogPage["PublishedDate1"].ToString(), out DateTime publishedDate);

            return new BlogPageModel
            {
                PageName = blogPage["PageName"] as string,
                Title = blogPage["Title"] as string,
                HeroId = (blogPage["Hero"] as FieldLookupValue).GetLookupFieldValue(),
                PublishedDate = hasDate ? publishedDate : DateTime.MinValue,
                HtmlText = (blogPage["Text"] as string).GetHtmlContent(),
            };
        }

        public async Task<BlogModel> GetBlogComponentModelAsync(IEnumerable<ListItem> blogPages, int take, ClientContext ctx)
        {
            if (blogPages == null)
                return null;

            var model = new BlogModel
            {
                BlogPosts = new List<BlogPost>()
            };

            foreach (var page in blogPages)
            {
                var base64Image = string.Empty;
                var hasDate = DateTime.TryParse(page["PublishedDate1"].ToString(), out DateTime publishedDate);
                var heroId = (page["Hero"] as FieldLookupValue).GetLookupFieldValue();
                var hero = await ctx.GetHeroByIdAsync(heroId);

                if (publishedDate > DateTime.Now || publishedDate == DateTime.MinValue)
                    continue;

                if (hero != null)
                {
                    var attachments = ctx.LoadQuery(hero.AttachmentFiles.Include(a => a.ServerRelativeUrl));

                    await ctx.ExecuteQueryAsync();

                    var image = attachments.FirstOrDefault();

                    if (image != null)
                    {
                        base64Image = await ctx.GetImageAsBase64String(image.ServerRelativeUrl);
                    }
                }

                model.BlogPosts.Add(new BlogPost
                {
                    BlogPageId = page.Id,
                    Title = page["PageName"] as string,
                    PublishedDate = hasDate ? publishedDate : DateTime.MinValue,
                    Image = base64Image,
                    Teaser = (page["Text"] as string).StripAndTruncateHtml(150) + "..."
                });
            }

            model.BlogPosts = model.BlogPosts.OrderByDescending(b => b.PublishedDate)
                                             .Take(take)
                                             .ToList();

            return model;
        }

        public async Task<AboutModel> GetAboutModelAsync(ListItem listItem, ClientContext ctx)
        {
            if (listItem == null)
                return null;

            var base64Image = string.Empty;
            var attachments = ctx.LoadQuery(listItem.AttachmentFiles.Include(a => a.ServerRelativeUrl));

            await ctx.ExecuteQueryAsync();

            var image = attachments.FirstOrDefault();

            if (image != null)
            {
                base64Image = await ctx.GetImageAsBase64String(image.ServerRelativeUrl);
            }

            return new AboutModel
            {
                ComponentId = listItem.Id,
                ComponentName = listItem["ComponentName"] as string,
                Title = listItem["Title"] as string,
                HtmlText = (listItem["Text"] as string).GetHtmlContent(),
                Image = base64Image
            };
        }

        public async Task<HighlightsModel> GetHighlightsModelAsync(ListItem listItem, ClientContext ctx)
        {
            if (listItem == null)
                return null;

            var model = new HighlightsModel
            {
                ComponentId = listItem.Id,
                ComponentName = listItem["ComponentName"] as string,
                Highlights = new List<Highlight>()
            };

            var lookupIds = (listItem["Highlights"] as IEnumerable<FieldLookupValue>).Select(x => x.LookupId);

            if (lookupIds.Any())
            {
                var highlightsList = ctx.Web.Lists.GetByTitle("Data__Highlights");
                var allHighlights = ctx.LoadQuery(highlightsList.GetItems(CamlQuery.CreateAllItemsQuery())
                                                                .QueryHighlights());
                await ctx.ExecuteQueryAsync();

                foreach (var highlight in allHighlights.Where(h => lookupIds.Any(id => id == h.Id)))
                {
                    var base64Image = string.Empty;
                    var attachments = ctx.LoadQuery(highlight.AttachmentFiles.Include(a => a.ServerRelativeUrl));

                    await ctx.ExecuteQueryAsync();

                    var image = attachments.FirstOrDefault();

                    if (image != null)
                    {
                        base64Image = await ctx.GetImageAsBase64String(image.ServerRelativeUrl);
                    }

                    model.Highlights.Add(new Highlight
                    {
                        Title = highlight["Title"] as string,
                        HtmlText = (highlight["Text"] as string).GetHtmlContent(),
                        Image = base64Image
                    });
                }
            }

            return model;
        }

        public async Task<SkillsModel> GetSkillsModelAsync(ListItem listItem, ClientContext ctx)
        {
            if (listItem == null)
                return null;

            var model = new SkillsModel
            {
                ComponentId = listItem.Id,
                ComponentName = listItem["ComponentName"] as string,
                Skills = new List<Skill>()
            };

            var lookupIds = (listItem["Skills"] as IEnumerable<FieldLookupValue>).Select(x => x.LookupId);

            if (lookupIds.Any())
            {
                var skillsList = ctx.Web.Lists.GetByTitle("Data__Skills");
                var items = skillsList.GetItems(CamlQuery.CreateAllItemsQuery());
                var allSkills = ctx.LoadQuery(items.QuerySkills());

                await ctx.ExecuteQueryAsync();

                foreach (var skill in allSkills.Where(s => lookupIds.Any(id => id == s.Id)))
                {
                    var hasSkillLevel = float.TryParse(skill["SkillLevel"].ToString(), out float skillLevel);
                    if (hasSkillLevel)
                    {
                        model.Skills.Add(new Skill
                        {
                            Name = skill["SkillName"] as string,
                            Level = skillLevel
                        });
                    }
                }
            }

            model.Skills = model.Skills.OrderByDescending(s => s.Level)
                                       .ToList();
            return model;
        }

        public ContactModel GetContactFormModel(ListItem listItem)
        {
            return new ContactModel
            {
                ComponentId = listItem.Id,
                ComponentName = listItem["ComponentName"] as string,
                SendMailTo = listItem["SendToEmail"] as string,
                HtmlText = (listItem["Text"] as string).GetHtmlContent()
            };
        }

        public async Task<ProjectsModel> GetProjectsModelAsync(ListItem listItem, ClientContext ctx)
        {
            if (listItem == null)
                return null;

            var model = new ProjectsModel
            {
                ComponentId = listItem.Id,
                ComponentName = listItem["ComponentName"] as string,
                Projects = new List<Project>()
            };

            var lookupIds = (listItem["Projects"] as IEnumerable<FieldLookupValue>).Select(x => x.LookupId);

            if (lookupIds.Any())
            {
                var projectsList = ctx.Web.Lists.GetByTitle("Data__Projects");
                var allProjects = ctx.LoadQuery(projectsList.GetItems(CamlQuery.CreateAllItemsQuery())
                                                            .QueryProjects());
                await ctx.ExecuteQueryAsync();

                foreach (var project in allProjects.Where(p => lookupIds.Any(id => id == p.Id)))
                {
                    var base64Image = string.Empty;
                    var attachments = ctx.LoadQuery(project.AttachmentFiles.Include(a => a.ServerRelativeUrl));

                    await ctx.ExecuteQueryAsync();

                    var image = attachments.FirstOrDefault();

                    if (image != null)
                    {
                        base64Image = await ctx.GetImageAsBase64String(image.ServerRelativeUrl);
                    }

                    var projectLink = project["ProjectLink"] as FieldUrlValue;

                    model.Projects.Add(new Project
                    {
                        Title = project["Title"] as string,
                        Technology = project["Technology"] as string,
                        Image = base64Image,
                        Link = projectLink?.Url,
                        LinkText = projectLink?.Description
                    });
                }
            }

            return model;
        }
    }
}
