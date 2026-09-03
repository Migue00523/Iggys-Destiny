using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{

    [SerializeField] private Button start;
    [SerializeField] private Button options;

    public void OnClickStart()
    {
        Debug.Log("Iniciando juego. Cargando selector de niveles.");
        // Llamamos al gestor general de escenas para ir al pasillo de niveles
        SceneManager.LoadScene("SelectorNivel");
    }

    public void OnClickOptions()
    {
        Debug.Log("Abriendo menú de opciones...");
        SceneManager.LoadScene("");
    }

    public void OnClickBack()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "SelectorNivel")
        {
            SceneManager.LoadScene("Inicio");
        }
        else
        {
            SceneManager.LoadScene("SelectorNivel");
        }
    }
}