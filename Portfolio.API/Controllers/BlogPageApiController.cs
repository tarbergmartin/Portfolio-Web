using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SharePoint.Client;
using Portfolio.API.Classes;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers
{
    [Route("api/blogPages")]
    [ApiController]
    public class BlogPageApiController : ControllerBase
    {
        private readonly SharePointConfiguration _spConfiguration;
        private readonly ComponentModelFactory _modelFactory;


        public BlogPageApiController(SharePointConfiguration spConfiguration)
        {
            _spConfiguration = spConfiguration;
            _modelFactory = new ComponentModelFactory();
        }

        /// <summary>
        /// Fetches blog page by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            using (var ctx = new ClientContext(_spConfiguration.TargetSite))
            {
                ctx.Credentials = _spConfiguration.Credentials;

                try
                {
                    var pagesList = ctx.Web.Lists.GetByTitle("Pages");
                    var filteredBlogPages = ctx.LoadQuery(pagesList.GetItems(CamlQuery.CreateAllItemsQuery())
                                                                   .QueryBlogPagesById(id));
                    await ctx.ExecuteQueryAsync();

                    if (filteredBlogPages.Any())
                    {
                        var blogPage = filteredBlogPages.FirstOrDefault();
                        var blogPageModel = _modelFactory.GetBlogPageModel(blogPage);
                        return Ok(blogPageModel);
                    }
                }

                catch (Exception e)
                {
                    // Log error?
                    return StatusCode(500, "Something went wrong while trying to fetch the hero.");
                }
            }

            return NotFound(null);
        }
    }
}