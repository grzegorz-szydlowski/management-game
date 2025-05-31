using System.Collections.Generic;
using System.Linq;
using Core.Data.DTO;
using Core.Data.Models;
using Core.Interfaces;
using UnityEngine;
using Newtonsoft.Json;


namespace Core
{
    public class ScenarioManager :ISystem
    {
        private static ScenarioManager Instance;
        public TextAsset scenarioTextAsset;
        public TextAsset randomEventTextAsset;
        public List<Scenario> Scenarios;
        public List<RandomEvent> RandomEvents;

        private ScenarioManager() { }

        public static ScenarioManager CreateInstance()
        {
            if (Instance == null)
            {
                Instance = new ScenarioManager();
            }
            return Instance;
        }

        public void Initialize()
        {
            scenarioTextAsset = Resources.Load<TextAsset>("GameplayScenarios/scenarios");
            randomEventTextAsset = Resources.Load<TextAsset>("GameplayScenarios/randomEvents");
            Scenarios = JsonConvert.DeserializeObject<List<Scenario>>(scenarioTextAsset.text);
            RandomEvents = JsonConvert.DeserializeObject<List<RandomEvent>>(randomEventTextAsset.text);
        }

        public void Shutdown()
        {
            Debug.Log("ScenarioLoadingManager Shutdown.");
        }

        public List<TaskDTO> GetTasksForDisplayByScenarioAndStageId(int scenarioId, int stageId)
        {
            return Scenarios.FirstOrDefault(scenario=>scenario.Id == scenarioId).Stages.FirstOrDefault(stage => stage.Id == stageId).Tasks.Select(
                task => new TaskDTO
                {
                    Id = task.Id,
                    CostPerHour = task.CostPerHour,
                    Title = task.Name,
                }).ToList();
        }

        public List<Reaction> GetReactionsByEventId(int eventId)
        {
            return RandomEvents.Where(x => x.Id == eventId).FirstOrDefault().Reactions;
        }
        
        public List<int> GetRandomEventsIds()
        {
            return RandomEvents.Select(x => x.Id).ToList();
        }

        public RandomEvent GetRandomEventById(int id)
        {
            return RandomEvents.FirstOrDefault(x => x.Id == id);
        }
        public List<ScenarioDTO> GetScenariosForDisplay()
        {
           return Scenarios.Select(scenario => new ScenarioDTO{Id=scenario.Id,Title=scenario.Title}).ToList();
        }

        public Scenario GetScenarioById(int scenarioId)
        {
            return Scenarios.FirstOrDefault(scenario => scenario.Id == scenarioId);
        }
    }
}