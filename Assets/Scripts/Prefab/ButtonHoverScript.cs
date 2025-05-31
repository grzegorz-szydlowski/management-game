using Core;
using Core.GameSystems;
using Core.GameSystems.Managers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Prefab
{
    [RequireComponent(typeof(Button))]
    public class ButtonHoverScript : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private AudioClip hoverClip;
        private AudioManager audioManager;

        private void Start()
        {
            audioManager = SystemProvider.GetSystem<AudioManager>();
            if (hoverClip == null)
            {
                hoverClip = Resources.Load<AudioClip>("SFX/FastUiSounds/SFX_FastUiChangeOption_01_wav");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hoverClip != null && audioManager != null)
            {
                audioManager.PlaySFX(hoverClip);
            }
        }
    }
}