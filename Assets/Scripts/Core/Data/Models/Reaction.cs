using System;

namespace Core.Data.Models
{
    [Serializable]
    public class Reaction
    {
        public string Action { get; set; }
        public bool Randomize { get; set; }
        public int Morale { get; set; }
        public int Budget { get; set; }
        public int Time { get; set; }
    }
}