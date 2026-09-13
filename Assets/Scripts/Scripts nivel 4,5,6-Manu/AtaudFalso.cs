using UnityEngine;

public class AtaudFalso : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject ataudVerdadero;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (ataudVerdadero != null)
        {
            ataudVerdadero.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}