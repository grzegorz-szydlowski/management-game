using System;

namespace Core.Data.Models
{
    [Serializable]
    public class Task
    {
        public int Id { get; set; }
        public string ProjectStage { get; set; }
        public string Name { get; set; }
        public int CostPerHour { get; set; }
        public int BobTime { get; set; }
        public int MarkTime { get; set; }
        public int DanTime { get; set; }
        public int JennyTime { get; set; }
        public int BobMoraleEffect { get; set; }
        public int MarkMoraleEffect { get; set; }
        public int DanMoraleEffect { get; set; }
        public int JennyMoraleEffect { get; set; }
    }
}