using System;
using System.Collections.Generic;

namespace Core.Data.Models
{
    [Serializable]
    public class RandomEvent
    {
        public int Id {get;set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Reaction> Reactions { get; set; }
    }
}