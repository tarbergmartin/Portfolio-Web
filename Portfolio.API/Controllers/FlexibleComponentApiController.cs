using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using Portfolio.API.Classes;
using Portfolio.API.Models;
using Portfolio.Shared.Interfaces;

namespace Portfolio.API.Controllers
{
    [Route("api/flexComponent")]
    [ApiController]
    public class FlexibleComponentApiController : ControllerBase
    {
        private readonly SharePointConfiguration _spConfiguration;
        private readonly ComponentModelFactory _modelFactory;

        public FlexibleComponentApiController(SharePointConfiguration spConfiguration)
        {
            _spConfiguration = spConfiguration;
            _modelFactory = new ComponentModelFactory();
        }

        /// <summary>
        /// Fetches flexible component by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            using (var ctx = new ClientContext(_spConfiguration.TargetSite))
            {
                ctx.Credentials = _spConfiguration.Credentials;

                try
                {
                    var flexibleList = ctx.Web.Lists.GetByTitle("FlexibleComponents");
                    var filteredFlexComponents = ctx.LoadQuery(flexibleList.GetItems(CamlQuery.CreateAllItemsQuery())
                                                                           .QueryByFlexibleListItemId(id));
                    await ctx.ExecuteQueryAsync();

                    if (filteredFlexComponents.Any())
                    {
                        var flexComponent = filteredFlexComponents.FirstOrDefault();
                        IComponentModel componentModel = flexComponent.ContentType.Name switch
                        {
                            "AboutMeComponent" => await _modelFactory.GetAboutModelAsync(flexComponent, ctx),
                            "ContactFormComponent" => _modelFactory.GetContactFormModel(flexComponent),
                            "HighlightComponent" => await _modelFactory.GetHighlightsModelAsync(flexComponent, ctx),
                            "ProjectsComponent" => await _modelFactory.GetProjectsModelAsync(flexComponent, ctx),
                            "SkillsComponent" => await _modelFactory.GetSkillsModelAsync(flexComponent, ctx),
                            _ => null
                        };

                        // Add settings to ensure proper deserialization of interface in receiving application
                        var serializedModel = JsonConvert.SerializeObject(componentModel, Formatting.Indented, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });

                        return Ok(serializedModel);
                    }
                }

                catch (Exception e)
                {
                    return StatusCode(500, "Something went wrong while trying to fetch a flexible component");
                }
            }

            return NotFound();
        }
    }
}