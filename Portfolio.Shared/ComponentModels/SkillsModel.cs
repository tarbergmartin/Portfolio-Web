using Portfolio.Shared.DataModels;
using Portfolio.Shared.Interfaces;
using System.Collections.Generic;

namespace Portfolio.Shared.ComponentModels
{
    public class SkillsModel : IComponentModel
    {
        public List<Skill> Skills { get; set; }
    }
}
