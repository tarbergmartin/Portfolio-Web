using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SharePoint.Client;
using Portfolio.API.Classes;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers
{
    [Route("api/blogComponent")]
    [ApiController]
    public class BlogComponentApiController : ControllerBase
    {
        private readonly SharePointConfiguration _spConfiguration;
        private readonly ComponentModelFactory _modelFactory;

        public BlogComponentApiController(SharePointConfiguration spConfiguration)
        {
            _spConfiguration = spConfiguration;
            _modelFactory = new ComponentModelFactory();
        }

        /// <summary>
        /// Fetches all blog pages and returns the BlogModel used on the start page
        /// </summary>
        [HttpGet("{take}")]
        public async Task<IActionResult> Get(int take = 4)
        {
            using (var ctx = new ClientContext(_spConfiguration.TargetSite))
            {
                ctx.Credentials = _spConfiguration.Credentials;

                try
                {
                    var pagesList = ctx.Web.Lists.GetByTitle("Pages");
                    var blogPosts = ctx.LoadQuery(pagesList.GetItems(CamlQuery.CreateAllItemsQuery())
                                                           .QueryBlogPosts());
                    await ctx.ExecuteQueryAsync();

                    if (blogPosts.Any())
                    {
                        var blogComponentModel = await _modelFactory.GetBlogComponentModelAsync(blogPosts, take, ctx);
                        return Ok(blogComponentModel);
                    }
                }

                catch (Exception e)
                {
                    // Log error?
                    return StatusCode(500, "Something went wrong while trying to fetch the homepage.");
                }
            };

            return NotFound(null);
        }
    }
}