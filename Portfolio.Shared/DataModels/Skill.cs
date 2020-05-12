using System;

namespace Portfolio.Shared.DataModels
{
    public class Skill
    {
        public string Name { get; set; }
        public float Level { get; set; }
        public string LevelAsPercentageString => $"{Math.Round(Level * 100, 0)}%";
    }
}
