using Core;
using Core.GameSystems;
using Core.GameSystems.Managers;
using UnityEngine;

namespace Prefab
{
    public class CursorBehaviorScript : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private Texture2D cursorTexture;
        private Texture2D cursorDownTexture;
        private AudioClip clickSound;
        private AudioManager audioManager;

        void Start()
        {
            audioManager = SystemProvider.GetSystem<AudioManager>();
            cursorTexture = Resources.Load("Sprites/PixelCursors/Cursors/cursor") as Texture2D;
            cursorDownTexture = Resources.Load("Sprites/PixelCursors/Cursors/cursorClick") as Texture2D;
            clickSound = Resources.Load("SFX/FastUiSounds/SFX_FastUiClick_10_wav") as AudioClip;
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Click();
            else if (Input.GetKeyUp(KeyCode.Mouse0))
                Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }

        private void Click()
        {
            Cursor.SetCursor(cursorDownTexture, Vector2.zero, CursorMode.ForceSoftware);
            audioManager.PlaySFX(clickSound);
        }
    }
}