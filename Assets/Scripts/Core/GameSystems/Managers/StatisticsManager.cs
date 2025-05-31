using Core.Interfaces;

namespace Core.GameSystems.Managers
{
    public class StatisticsManager :ISystem
    {
        public static StatisticsManager Instance;
        private StatisticsManager(){}

        public static StatisticsManager CreateInstance()
        {
            if (Instance == null)
            {
                Instance = new StatisticsManager();
            }
            return Instance;
        }
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void Shutdown()
        {
            throw new System.NotImplementedException();
        }
    }
}