using Portfolio.Shared.DataModels;
using Portfolio.Shared.Interfaces;
using System.Collections.Generic;

namespace Portfolio.Shared.ComponentModels
{
    public class HighlightsModel : IComponentModel
    {
        public List<Highlight> Highlights { get; set; }
    }
}
