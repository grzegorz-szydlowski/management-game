using Core;
using Core.GameSystems;
using Core.GameSystems.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
    public class StatisticsCanvasScript : MonoBehaviour
    {
        private StatisticsManager statisticsManager;
        private SceneControlManager sceneControlManager;
        private DbManager dbManager;

        private Image backgroundImage;

        private Button goBackButton;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            backgroundImage = transform.Find("BackgroundImage")?.GetComponent<Image>();
            if (backgroundImage == null) return;
            Sprite backgroundSprite = Resources.Load<Sprite>("Backgrounds/statsBg_big");
            backgroundImage.sprite = backgroundSprite;
            goBackButton = transform.Find("GoBackButton")?.GetComponent<Button>();
            if (goBackButton == null) Debug.LogError("GoBackButton not found");
            //statisticsManager=SystemProvider.GetSystem<StatisticsManager>();
            sceneControlManager = SystemProvider.GetSystem<SceneControlManager>();
            goBackButton.onClick.AddListener(GoBack);

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void GoBack()
        {
            Debug.Log("GoBack");
            sceneControlManager.ReturnToMainMenu();
        }
    }
}
