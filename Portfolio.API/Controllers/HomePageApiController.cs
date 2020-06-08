using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SharePoint.Client;
using Portfolio.API.Classes;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers
{
    [Route("api/homePage")]
    [ApiController]
    public class HomePageApiController : ControllerBase
    {
        private readonly SharePointConfiguration _spConfiguration;
        private readonly ComponentModelFactory _modelFactory;

        public HomePageApiController(SharePointConfiguration spConfiguration)
        {
            _spConfiguration = spConfiguration;
            _modelFactory = new ComponentModelFactory();
        }

        /// <summary>
        /// Fetches the first home page in SharePoint "Pages" list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var ctx = new ClientContext(_spConfiguration.TargetSite))
            {
                ctx.Credentials = _spConfiguration.Credentials;
                
                try
                {
                    var pagesList = ctx.Web.Lists.GetByTitle("Pages");
                    var homePages = ctx.LoadQuery(pagesList.GetItems(CamlQuery.CreateAllItemsQuery())
                                                           .QueryHomePages());
                    await ctx.ExecuteQueryAsync();

                    if (homePages.Any())
                    {
                        var homePage = homePages.FirstOrDefault();
                        var homePageModel = _modelFactory.GetHomePageModel(homePage);
                        return Ok(homePageModel);
                    }
                }

                catch (Exception e)
                {
                    // Log error?
                    return StatusCode(500, "Something went wrong while trying to fetch the homepage.");
                }
            }

            return NotFound();
        }
    }
}