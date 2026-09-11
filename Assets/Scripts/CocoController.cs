using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Enemies
{
    public class CrocodileController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private Transform leftPoint;
        [SerializeField] private Transform rightPoint;

        private bool movingRight = true;

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (movingRight)
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;

                if (transform.position.x >= rightPoint.position.x)
                {
                    movingRight = false;
                    Flip();
                }
            }
            else
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;

                if (transform.position.x <= leftPoint.position.x)
                {
                    movingRight = true;
                    Flip();
                }
            }
        }

        private void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerCaught();
            }
        }

        private void PlayerCaught()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}