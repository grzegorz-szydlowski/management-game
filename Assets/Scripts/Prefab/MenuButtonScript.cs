using System;
using Enumerations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Prefab
{
    public class MenuButtonScript : MonoBehaviour
    {
        public MenuButtonType buttonType;
        private Button button;
        public string buttonText;
        public Sprite backgroundSprite;
        public TMP_FontAsset font;
        private TextMeshProUGUI buttonTextComponent;

        void Start()
        {
            button = gameObject.GetComponent<Button>();
            if (button == null) return;
            backgroundSprite = Resources.Load<Sprite>("Sprites/ExtraCleanUI/Blue");
            font = Resources.Load<TMP_FontAsset>("Thaleah_PixelFont/Materials/ThaleahFat_TTF SDF");
            buttonTextComponent = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonTextComponent.fontSize = 100;
            switch (buttonType)
            {
                case MenuButtonType.StartButton:
                    buttonText = "START";
                    buttonTextComponent.fontSize = 150;
                    break;
                case MenuButtonType.ExitButton: buttonText = "EXIT"; break;
                case MenuButtonType.StatisticsButton: buttonText = "STATS"; break;
                case MenuButtonType.PlayButton: buttonText = "PLAY"; break;
            }

            buttonTextComponent.text = buttonText;
            buttonTextComponent.font = font;
            button.image.sprite = backgroundSprite;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}