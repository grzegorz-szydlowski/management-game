using System.Collections.Generic;
using System.Linq;
using Core.Data.Models;
using Core.GameSystems.Managers;
using Core.Interfaces;
using UnityEngine;

namespace Core.GameSystems
{
    public class GameManagerScript : MonoBehaviour
    {
        public static GameManagerScript Instance;
        public static List<ISystem> Systems;
        public GameState currentGameState;
        private void Awake()
        {
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            InitializeSystems();
            InitializeEmptyGameState();
            var sceneControlManager = Systems.OfType<SceneControlManager>().FirstOrDefault();
            if (sceneControlManager != null && sceneControlManager.GetCurrentSceneName() == "InitScene")
            {
                sceneControlManager.LoadScene("MenuScene");
            }
            
        }

        private static void InitializeEmptyGameState()
        {
            Instance.currentGameState = new GameState();
        }
    
        private static void InitializeSystems()
        {
            var dbManager = DbManager.CreateInstance();
            SystemProvider.Register(dbManager);
            var sceneControlManager = SceneControlManager.CreateInstance();
            SystemProvider.Register(sceneControlManager);
            var audioManager = AudioManager.CreateInstance();
            SystemProvider.Register(audioManager);
            var scenarioManager = ScenarioManager.CreateInstance();
            SystemProvider.Register(scenarioManager);
            // var statisticsManager = StatisticsManager.CreateInstance();
            // SystemProvider.Register(statisticsManager);
            Systems = new() { dbManager, sceneControlManager, audioManager, scenarioManager };//,statisticsManager };
            foreach (var system in Systems)
            {
                system.Initialize();
            }
        }
    
        private void OnApplicationQuit()
        {
            foreach (var system in Systems)
            {
                system.Shutdown();
            }
            SystemProvider.Clear();
        }
    }

}
