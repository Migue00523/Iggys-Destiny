using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI {
    public class CargarEscenas : MonoBehaviour {
        public void LoadScene(string sceneName) {
            if (string.IsNullOrEmpty(sceneName)) {
                Debug.LogError("CargarEscenas: El nombre de la escena esta vacio.");
                return;
            }

            SceneManager.LoadScene(sceneName);
        }
        public void LoadSceneByIndex(int sceneIndex) {
            if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings) {
                Debug.LogError($"CargarEscenas: Índice de escena inválido: {sceneIndex}");
                return;
            }

            SceneManager.LoadScene(sceneIndex);
        }
        public void LoadNextScene() {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            LoadSceneByIndex(nextIndex);
        }
        public void ReloadScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}