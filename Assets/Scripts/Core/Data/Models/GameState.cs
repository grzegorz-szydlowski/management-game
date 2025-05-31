namespace Core.Data.Models
{
    public class GameState
    {
        public int ScenarioId = 0;
        public Player player = new();
        public int Time { get; set; } = 0;
        public int Budget { get; set; } = 0;
        public int Morale { get; set; } = 100;
        public int Stage { get; set; } = 1;
    }
}