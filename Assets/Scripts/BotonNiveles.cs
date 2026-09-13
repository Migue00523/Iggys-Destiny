using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonNiveles : MonoBehaviour
{
    public string nivelesSceneName = "Selectornivel";

    public void IrANiveles()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nivelesSceneName);
    }
}