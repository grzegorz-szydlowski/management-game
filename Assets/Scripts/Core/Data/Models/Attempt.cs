namespace Core.Data.Models
{
    public class Attempt
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string ScenarioName { get; set; }
        public int Time { get; set; }
        public int Budget { get; set; }
        public int Morale { get; set; }
    }
}