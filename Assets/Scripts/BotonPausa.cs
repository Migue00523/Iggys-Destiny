using UnityEngine;

public class BotonPausa : MonoBehaviour
{
    public void Pausar()
    {
        Time.timeScale = 0f;
    }
}