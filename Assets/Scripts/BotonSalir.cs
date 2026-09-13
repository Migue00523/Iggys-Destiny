using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonSalir : MonoBehaviour
{
    public void Salir()
    {
        Time.timeScale = 1f; // por si el juego estaba en pausa
        SceneManager.LoadScene("Inicio");
    }
}