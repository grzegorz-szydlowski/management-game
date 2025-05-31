using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using Core.Data.DTO;
using Core.Data.Models;
using Core.GameSystems;
using Core.GameSystems.Managers;
using Enumerations;
using Prefab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
    
    public class GameplayCanvasScript : MonoBehaviour
    {
        private StatisticsManager statisticsManager;
        private SceneControlManager sceneControlManager;
        private AudioManager audioManager;
        private DbManager dbManager; 
        private Scenario currentScenario;
        private ScenarioManager scenarioManager;
        
        [SerializeField] private Sprite mutedSprite;
        [SerializeField] private Sprite unmutedSprite;
        private AssignTaskPanelScript assignTasksPanelScript;
        private GameSummaryScript gameSummaryScript;
        private Image muteButtonImage;
        private Image helpPanel;
        private Image backgroundImage;
        private Button settingsButton;
        private Button okButton;
        private Image initialInstructionPanel;
        private TMP_Text instructionText;
        private TMP_Text helpText;
        private List<Button> settingsButtons = new();
        private TMP_Text newStageText;
        private HUDBehaviorScript hudBehaviorScript;
        [SerializeField] private GameObject assignTasksPanel;
        [SerializeField] private GameObject randomEventPanelPrefab;
        private RandomEventPanelScript randomEventPanelScript;
        
        private GamePhase currentPhase;
        private bool isTransitioning;

        private List<int> randomEvents;
        
        private bool areSettingsVisible = false;
        private bool isHelpPanelVisible = false;
        private int currentStage;
        private int morale;
        private int budget;
        private int time;


        private void Start()
        {
            //Assigning game systems in a dependency-injection style
            sceneControlManager=SystemProvider.GetSystem<SceneControlManager>();
            audioManager=SystemProvider.GetSystem<AudioManager>();
            scenarioManager=SystemProvider.GetSystem<ScenarioManager>();
            
            //Assign phase
            currentPhase = GamePhase.InitialInstructions;
            
            //Assign current scenario and game progress tracking variables from gamestate for easier access
            currentScenario = scenarioManager.GetScenarioById(GameManagerScript.Instance.currentGameState.ScenarioId);
            currentStage = GameManagerScript.Instance.currentGameState.Stage;
            morale = GameManagerScript.Instance.currentGameState.Morale;
            budget = GameManagerScript.Instance.currentGameState.Budget;
            time= GameManagerScript.Instance.currentGameState.Time;
            
            //Loading sprites and images
            backgroundImage = transform.Find("BackgroundImage")?.GetComponent<Image>();
            if (backgroundImage == null) return;
            Sprite backgroundSprite = Resources.Load<Sprite>("Backgrounds/gameplayBg_big");
            backgroundImage.sprite = backgroundSprite;
            unmutedSprite = Resources.Load<Sprite>("Sprites/MulticoloredGUIbuttons/Volume/Volume_MAX_ELLIPSE_ 5");
            mutedSprite = Resources.Load<Sprite>("Sprites/MulticoloredGUIbuttons/Volume_Mute/Volume_Mute_Ellip_ 2");
            
            //Init of settings buttons
            settingsButton = transform.Find("SettingsButton")?.GetComponent<Button>();
            if (settingsButton != null)
                settingsButton.onClick.AddListener(DisplaySettings);

            RegisterButton("HelpButton", Help);
            RegisterButton("GoBackButton", GoBack);
            RegisterButton("MuteButton", Mute);

            foreach (var button in settingsButtons)
            {
                button.gameObject.SetActive(false);
            }
            
            //Setup of help and init text loaded from scenario
            var helpAndInitText = currentScenario.Description;
            var sb = new StringBuilder(helpAndInitText);
            foreach (var employee in currentScenario.Employees)
            {
                sb.AppendLine();
                sb.AppendLine(employee.Description);
            }
            
            //Initial instructions init
            initialInstructionPanel = transform.Find("InitialInstructionPanel")?.GetComponent<Image>();
            okButton = initialInstructionPanel.transform.Find("OkButton")?.GetComponent<Button>();
            instructionText = initialInstructionPanel.transform.Find("Scroll View/Viewport/Content")?.GetComponent<TMP_Text>();
            instructionText.text = sb.ToString();
            okButton.onClick.AddListener(Ok);
            
            //Help panel init
            helpPanel = transform.Find("HelpPanel")?.GetComponent<Image>();
            helpText = helpPanel.transform.Find("Scroll View/Viewport/Content").GetComponent<TMP_Text>();
            helpText.text = sb.ToString();
            helpPanel.gameObject.SetActive(false);
            
            //HUD object for later updates
            hudBehaviorScript  = transform.Find("HUD")?.GetComponent<HUDBehaviorScript>();
            
            //Stage change text init
            newStageText = transform.Find("NewStageText")?.GetComponent<TMP_Text>();
            newStageText.gameObject.SetActive(false);
            
            //Assign tasks panel init
            GameObject assignTasksPanelGO = Instantiate(assignTasksPanel, transform);
            assignTasksPanelScript = assignTasksPanelGO.GetComponent<AssignTaskPanelScript>();
            assignTasksPanelScript.Initialize(ReceiveTaskAssignments);
            assignTasksPanelScript.gameObject.SetActive(false);
            
            //Random event panel init
            GameObject randomEventPanelGO = Instantiate(randomEventPanelPrefab, transform);
            randomEventPanelScript = randomEventPanelGO.GetComponent<RandomEventPanelScript>();
            randomEventPanelGO.SetActive(false);
            randomEvents = scenarioManager.GetRandomEventsIds();

        }

        private void UpdateHUD()
        {
            hudBehaviorScript.UpdateHUD(morale, currentStage,budget,time);
        }
        
        private void AdvanceGameState()
        {
            switch (currentPhase)
            {
                case GamePhase.InitialInstructions:
                    StartCoroutine(ShowStageIntroText());
                    Destroy(initialInstructionPanel.gameObject);
                    currentPhase = GamePhase.NewStageText;
                    break;

                case GamePhase.NewStageText:
                    ShowTaskAssignment();
                    currentPhase = GamePhase.TaskAssignment;
                    break;

                case GamePhase.TaskAssignment:
                    StartCoroutine(PerformTasks());
                    currentPhase = GamePhase.PerformingTasks;
                    break;

                case GamePhase.PerformingTasks:
                    StartCoroutine(HandleRandomEvents());
                    currentPhase = GamePhase.RandomEvents;
                    break;

                case GamePhase.RandomEvents:
                    currentStage++;
                    if (currentStage <= currentScenario.Stages.Count)
                    {
                        StartCoroutine(ShowStageIntroText());
                        currentPhase = GamePhase.NewStageText;
                    }
                    else
                    {
                        currentPhase = GamePhase.GameCompleted;
                        HandleGameCompletion();
                    }
                    break;
            }
        }

        
        private void ShowTaskAssignment()
        {
            assignTasksPanelScript.gameObject.SetActive(true);
            var tasks = scenarioManager.GetTasksForDisplayByScenarioAndStageId(currentScenario.Id,currentStage);
            assignTasksPanelScript.Populate(tasks);
        }
        
        private IEnumerator PerformTasks()
        {
            assignTasksPanelScript.gameObject.SetActive(false);
            Debug.Log("Tasks are now being performed...");
            float taskDuration = 5f;
            yield return new WaitForSeconds(taskDuration);
            AdvanceGameState();
        }
        
        private IEnumerator HandleRandomEvents()
        {
            float delay = Random.Range(3f, 7f);
            Debug.Log("Waiting for random event...");
            yield return new WaitForSeconds(delay);


            if (randomEvents.Count == 0) {
                AdvanceGameState();
                yield break;
            }

            int index = Random.Range(0, randomEvents.Count);
            int eventId = randomEvents[index];
            var selectedEvent = scenarioManager.GetRandomEventById(eventId);
            randomEvents.RemoveAt(index);
    
            randomEventPanelScript.gameObject.SetActive(true);
            randomEventPanelScript.Setup(selectedEvent, ReceiveRandomEventDecision);
        }

        
        private void HandleGameCompletion()
        {
            
        }


        
        private IEnumerator ShowStageIntroText()
        {
            newStageText.text = $"Stage {currentStage} - {currentScenario.Stages[currentStage - 1].Title}";
            newStageText.gameObject.SetActive(true);

            float duration = 1f;
            float visibleTime = 2f;
            CanvasGroup cg = newStageText.GetComponent<CanvasGroup>();
            if (cg == null) cg = newStageText.gameObject.AddComponent<CanvasGroup>();

            // Fade in
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                cg.alpha = t / duration;
                yield return null;
            }
            cg.alpha = 1;
            yield return new WaitForSeconds(visibleTime);

            // Fade out
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                cg.alpha = 1 - (t / duration);
                yield return null;
            }
            cg.alpha = 0;
            newStageText.gameObject.SetActive(false);

            AdvanceGameState();
        }

        public void ReceiveRandomEventDecision(int eventId, Reaction reaction)
        {
            if (reaction.Randomize)
            {
                reaction = scenarioManager.GetReactionsByEventId(eventId)[Random.Range(1, 4)];
            }

            morale += reaction.Morale;
            time += reaction.Time;
            budget += reaction.Budget;

            UpdateHUD();

            randomEventPanelScript.gameObject.SetActive(false);

            AdvanceGameState();
        }

        public void ReceiveTaskAssignments(Dictionary<int, string> assignments)
        {
            foreach (var pair in assignments)
            {
                Debug.Log($"Task {pair.Key} assigned to {pair.Value}");
            }
            AdvanceGameState();
        }
        private void RegisterButton(string name, UnityEngine.Events.UnityAction action)
        {
            var button = transform.Find(name)?.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(action);
                settingsButtons.Add(button);
            }
        }
        
        private void DisplaySettings()
        {
            if (areSettingsVisible)
            {
                foreach (var button in settingsButtons)
                {
                    button.gameObject.SetActive(false);
                }
                areSettingsVisible = false;
            }
            else
            {
                foreach (var button in settingsButtons)
                {
                    button.gameObject.SetActive(true);
                }
                areSettingsVisible = true;
            }
            
        }

        private void Ok()
        {
            initialInstructionPanel.gameObject.SetActive(false);
            AdvanceGameState();
        }
        private void GoBack()
        {
            sceneControlManager.ReturnToMainMenu();
        }

        private void Mute()
        {
            muteButtonImage = transform.Find("MuteButton")?.GetComponent<Image>();
            if (muteButtonImage != null)
            {
                if (audioManager.IsMuted())
                {
                    muteButtonImage.sprite = unmutedSprite;
                    audioManager.Unmute();
                }
                else
                {
                    muteButtonImage.sprite = mutedSprite;
                    audioManager.Mute();
                }
            }
            
        }
        private void Help()
        {
            isHelpPanelVisible = !isHelpPanelVisible;
            helpPanel.gameObject.SetActive(isHelpPanelVisible);
            if (isHelpPanelVisible)
            {
                helpPanel.transform.SetAsLastSibling();
            }
        }

    }

}
