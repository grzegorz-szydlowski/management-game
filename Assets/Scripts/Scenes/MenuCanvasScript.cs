using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Data.DTO;
using Core.Data.Models;
using Core.GameSystems;
using Core.GameSystems.Managers;
using Enumerations;
using Prefab;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
    public class MenuCanvasScript : MonoBehaviour
    {
        private DbManager dbManager;
        private SceneControlManager sceneControlManager;
        private AudioManager audioManager;
        private ScenarioManager scenarioManager;

        private int currentPlayerIndex=0;
        private List<Player> players;

        private List<MenuButtonType> menuButtonTypes = new ()
        {
            MenuButtonType.PlayButton,
            MenuButtonType.StatisticsButton,
            MenuButtonType.ExitButton
        };

        private Color[] colors = {
            Color.yellow, Color.red, Color.cyan, Color.green
        };

        private Button muteButton;
        private Button helpButton;
        private TMP_Text currentPlayerText;
        public GameObject menuButtonPrefab;
        [SerializeField] private GameObject chooseScenarioPanelPrefab;
        
        [SerializeField] private Sprite mutedSprite;
        [SerializeField] private Sprite unmutedSprite;
        private Sprite rightArrowSprite;
        private Image muteButtonImage;
        private Image helpPanel;

        private TMP_FontAsset fontAsset;
        private TMP_Text instructionsText;
        private AudioClip backgroundMusic;
        
        private bool isHelpPanelVisible = false;
        
        
        void Start()
        {
            //Game systems init
            dbManager = SystemProvider.GetSystem<DbManager>();
            sceneControlManager = SystemProvider.GetSystem<SceneControlManager>();
            audioManager = SystemProvider.GetSystem<AudioManager>();
            scenarioManager = SystemProvider.GetSystem<ScenarioManager>();
            
            //Available player list init and current player index init
            players = dbManager.GetPlayers();
            try
            {
                var playerIndex =
                    players.IndexOf(players.FirstOrDefault(x => x.Id == GameManagerScript.Instance.currentGameState.player.Id));
                currentPlayerIndex = playerIndex
                     == -1
                        ? 0
                        : playerIndex;
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e.Message);
            }
            
            //Loading of sprites and music
            rightArrowSprite = Resources.Load<Sprite>("Sprites/ExtraCleanUI/ArrowRightBlue");
            fontAsset = Resources.Load<TMP_FontAsset>("Thaleah_PixelFont/Materials/ThaleahFat_TTF SDF");
            backgroundMusic = Resources.Load<AudioClip>("Music/8Bit Music - 062022/5. Track 5");
            unmutedSprite = Resources.Load<Sprite>("Sprites/MulticoloredGUIbuttons/Volume/Volume_MAX_ELLIPSE_ 5");
            mutedSprite = Resources.Load<Sprite>("Sprites/MulticoloredGUIbuttons/Volume_Mute/Volume_Mute_Ellip_ 2");

            
            //Starting playback
            audioManager.PlayMusic(backgroundMusic);

            //Start button init
            Button startButton = transform.Find("StartMenuButton")?.GetComponent<Button>();
            if (startButton != null)
            {
                if (sceneControlManager.wasStartClicked)
                {
                    StartClicked();
                }
                else
                {
                    startButton.onClick.AddListener(StartClicked);
                    sceneControlManager.wasStartClicked = true;
                }
                
            }
            
            //Help panel init
            helpPanel = transform.Find("HelpPanel")?.GetComponent<Image>();
            instructionsText = helpPanel.transform.Find("Scroll View/Viewport/Content")?.GetComponent<TMP_Text>();
            if (instructionsText != null) instructionsText.text = Resources.Load<TextAsset>("GameInstruction").text;
            helpPanel.gameObject.SetActive(false);
            
            //Help button init
            helpButton = transform.Find("HelpButton")?.GetComponent<Button>();
            if (helpButton != null) helpButton.onClick.AddListener(Help);
            
            //Mute button init
            muteButton = transform.Find("MuteButton")?.GetComponent<Button>();
            if (muteButton != null) muteButton.onClick.AddListener(Mute);
        }
        
        void StartClicked()
        {
            //Deactivation of start button
            GameObject startButton = transform.Find("StartMenuButton").gameObject;
            startButton.SetActive(false);
            
            //Init of menu buttons
            int yPosition = 150;
            foreach (MenuButtonType menuButtonType in menuButtonTypes)
            {
                GameObject menuButton = Instantiate(menuButtonPrefab, transform);
                menuButton.name = menuButtonType.ToString();
                RectTransform rectTransform = menuButton.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(-450, yPosition);
                rectTransform.sizeDelta = new Vector2(450, 125);
                yPosition -= 175;

                MenuButtonScript menuButtonScript = menuButton.GetComponent<MenuButtonScript>();
                menuButtonScript.buttonType = menuButtonType;

                Button button = menuButton.GetComponent<Button>();
                button.onClick.AddListener(() => HandleButtonClick(menuButtonType));
            }
            
            //Init of player choice text
            GameObject textGO = new GameObject("CurrentPlayerText", typeof(RectTransform));
            textGO.transform.SetParent(transform, false);

            RectTransform textRT = textGO.GetComponent<RectTransform>();
            textRT.sizeDelta = new Vector2(400, 100);
            textRT.anchoredPosition = new Vector2(300, -25);

            currentPlayerText = textGO.AddComponent<TextMeshProUGUI>();
            currentPlayerText.alignment = TextAlignmentOptions.Center;
            currentPlayerText.fontSize = 110;
            currentPlayerText.color = colors[0];
            currentPlayerText.font = fontAsset;
            currentPlayerText.text = "";
            
            //Init of left player choice navigation button
            GameObject leftArrow = new GameObject("LeftArrow");
            leftArrow.transform.SetParent(transform, false);
            Button leftButton = leftArrow.AddComponent<Button>();
            leftArrow.AddComponent<ButtonHoverScript>();
            Image leftImage = leftArrow.AddComponent<Image>();
            leftImage.sprite = rightArrowSprite;
            leftImage.rectTransform.localScale = new Vector3(-1, 1, 1);
            RectTransform leftRT = leftArrow.GetComponent<RectTransform>();
            leftRT.sizeDelta = new Vector2(125, 125);
            leftRT.anchoredPosition = new Vector2(40, -25);
            leftButton.onClick.AddListener(() =>
            {
                currentPlayerIndex = (currentPlayerIndex - 1 + players.Count) % players.Count;
                UpdateCurrentPlayerDisplay();
            });
            
            //Init of right player choice navigation button
            GameObject rightArrow = new GameObject("RightArrow");
            rightArrow.transform.SetParent(transform, false);
            Button rightButton = rightArrow.AddComponent<Button>();
            rightArrow.AddComponent<ButtonHoverScript>();
            Image rightImage = rightArrow.AddComponent<Image>();
            rightImage.sprite = rightArrowSprite;
            RectTransform rightRT = rightArrow.GetComponent<RectTransform>();
            rightRT.sizeDelta = new Vector2(125, 125);
            rightRT.anchoredPosition = new Vector2(560, -25);
            rightButton.onClick.AddListener(() =>
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                UpdateCurrentPlayerDisplay();
            });
            
            //Call update player display to display the first player correctly and instantiate the player in GameManager
            UpdateCurrentPlayerDisplay();
        }

        void UpdateCurrentPlayerDisplay()
        {
            if (players == null || players.Count == 0) return;
            GameManagerScript.Instance.currentGameState.player = players[currentPlayerIndex];
            currentPlayerText.text = GameManagerScript.Instance.currentGameState.player.Name;
            currentPlayerText.color = colors[currentPlayerIndex];
        }

        
        void HandleButtonClick(MenuButtonType menuButtonType)
        {
            switch (menuButtonType)
            {
                case MenuButtonType.PlayButton: PlayButtonClicked(); break;
                case MenuButtonType.StatisticsButton: StatisticsButtonClicked(); break;
                case MenuButtonType.ExitButton: ExitButtonClicked(); break;
            }
        }

        void ExitButtonClicked()
        {
            Debug.Log("Exit Game");
            Application.Quit();
        }

        void PlayButtonClicked()
        {
            GameObject panelGO = Instantiate(chooseScenarioPanelPrefab, transform);
            ChooseScenarioPanelScript panelScript = panelGO.GetComponent<ChooseScenarioPanelScript>();
            List<ScenarioDTO> scenarios = scenarioManager.GetScenariosForDisplay();
            panelScript.Populate(scenarios);
            
        }

        void StatisticsButtonClicked()
        {
            Debug.Log("Show Statistics");
            sceneControlManager.LoadScene("StatisticsScene");
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
