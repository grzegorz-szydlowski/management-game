using Core.Interfaces;
using UnityEngine;

namespace Core.GameSystems.Managers
{
    public class SceneControlManager: ISystem
    {
        private static SceneControlManager Instance;
        public bool wasStartClicked;
        private SceneControlManager(){}
        public static SceneControlManager CreateInstance()
        {
            if (Instance == null)
            {
                Instance = new SceneControlManager();
            }
            return Instance;
        }
        public void Initialize()
        {
            wasStartClicked = false;
        }

        public void Shutdown()
        {
            Debug.Log("SceneManager Shutdown.");
        }

        public void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public string GetCurrentSceneName()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
        
        public async void LoadSceneAsync(string sceneName)
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            while (!loading.isDone)
            {
                await System.Threading.Tasks.Task.Yield();
            }
        }
        
        public void ReturnToMainMenu()
        {
            LoadScene("MenuScene");
        }
    }
}