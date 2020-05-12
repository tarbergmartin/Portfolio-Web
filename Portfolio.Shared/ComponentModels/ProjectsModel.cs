using Portfolio.Shared.DataModels;
using Portfolio.Shared.Interfaces;
using System.Collections.Generic;

namespace Portfolio.Shared.ComponentModels
{
    public class ProjectsModel : IComponentModel
    {
        public List<Project> Projects { get; set; }
    }
}
