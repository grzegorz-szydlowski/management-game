using System;
using Core;
using Core.Data.DTO;
using Core.GameSystems;
using Core.GameSystems.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prefab
{
    public class ScenarioChoiceButtonScript : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Button button;

        private ScenarioDTO data;
        private ScenarioManager scenarioManager;
        private SceneControlManager sceneControlManager;

        private void Start()
        {
            sceneControlManager = SystemProvider.GetSystem<SceneControlManager>();
            scenarioManager = SystemProvider.GetSystem<ScenarioManager>();
        }

        public void Setup(ScenarioDTO scenario)
        {
            data = scenario;
            titleText.text = $"Scenario {scenario.Id} - {scenario.Title}";
            button.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            GameManagerScript.Instance.currentGameState.ScenarioId = data.Id;
            sceneControlManager.LoadScene("GameplayScene");
            Debug.Log($"Selected scenario: {data.Id}");
        }
    }
}

