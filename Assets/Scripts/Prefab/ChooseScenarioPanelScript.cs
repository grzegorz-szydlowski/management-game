using System;
using System.Collections.Generic;
using Core.Data.DTO;
using Prefab;
using UnityEngine;

namespace Prefab
{
    public class ChooseScenarioPanelScript : MonoBehaviour
    {
        [SerializeField] private Transform contentContainer;
        [SerializeField] private GameObject scenarioChoiceButtonPrefab;

        public void Populate(List<ScenarioDTO> scenarios)
        {
            scenarios.Add(new ScenarioDTO { Id = 2, Title = "New functionality" });
            scenarios.Add(new ScenarioDTO { Id = 3, Title = "Hiring a new member" });
            // Clear old children
            foreach (Transform child in contentContainer)
            {
                Destroy(child.gameObject);
            }

            // Instantiate a button for each scenario
            foreach (var scenario in scenarios)
            {
                GameObject buttonGO = Instantiate(scenarioChoiceButtonPrefab, contentContainer);
                buttonGO.AddComponent<ButtonHoverScript>();
                ScenarioChoiceButtonScript button = buttonGO.GetComponent<ScenarioChoiceButtonScript>();
                button.Setup(scenario);
            }
        }
    }
}

