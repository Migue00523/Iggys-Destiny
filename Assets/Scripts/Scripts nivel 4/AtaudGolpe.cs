using Game.Level;
using UnityEngine;

public class AtaudGolpe : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private DoorController puerta;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Rigidbody2D jugador = collision.gameObject.GetComponent<Rigidbody2D>();

        if (jugador != null && jugador.linearVelocity.y > 0)
        {
            puerta.Open();
        }
    }
}