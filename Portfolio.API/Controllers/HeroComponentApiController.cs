using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SharePoint.Client;
using Portfolio.API.Classes;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers
{
    [Route("api/heroComponent")]
    [ApiController]
    public class HeroComponentApiController : ControllerBase
    {
        private readonly SharePointConfiguration _spConfiguration;
        private readonly ComponentModelFactory _modelFactory;

        public HeroComponentApiController(SharePointConfiguration spConfiguration)
        {
            _spConfiguration = spConfiguration;
            _modelFactory = new ComponentModelFactory();
        }

        /// <summary>
        /// Fetches hero component by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            using (var ctx = new ClientContext(_spConfiguration.TargetSite))
            {
                ctx.Credentials = _spConfiguration.Credentials;

                try
                {
                    var heroItem = await ctx.GetHeroByIdAsync(id);
                    var heroModel = await _modelFactory.GetHeroModelAsync(heroItem, ctx);
                    return Ok(heroModel);
                }

                catch (Exception e)
                {
                    // Log error?
                    return StatusCode(500, "Something went wrong while trying to fetch the hero.");
                }
            }
        }
    }
}