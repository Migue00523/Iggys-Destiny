using UnityEngine;

public class BotonSalir : MonoBehaviour
{
    public void Salir()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}