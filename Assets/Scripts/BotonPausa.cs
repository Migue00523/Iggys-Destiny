using UnityEngine;

public class BotonPausa : MonoBehaviour
{
    bool pausado = false;
    public void Pausar()
    {
        if (pausado) {
            Time.timeScale = 1f;
            pausado = false;
        } else {
            Time.timeScale = 0f;
            pausado=true;
        }
    }
}