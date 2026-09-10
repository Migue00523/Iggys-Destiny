using UnityEngine;

public class GuardadoNivel : MonoBehaviour
{
    public int nivel;

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Player"))
            {
                if (PlayerPrefs.GetInt("Nivel")< nivel)
                    PlayerPrefs.SetInt("Nivel", nivel);
                    Debug.Log("Nivel guardado: " + nivel);
            }
    }
}
