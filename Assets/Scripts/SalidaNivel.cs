using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaNivel : MonoBehaviour
{
   
    public string SelectorNivel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
            Debug.Log("Saliendo al selector!");
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SelectorNivel);
    }
}
