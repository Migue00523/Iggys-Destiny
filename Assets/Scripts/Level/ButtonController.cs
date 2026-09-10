using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(Collider2D))]
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private DoorController door;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            if (door != null)
            {
                door.Open();
            }

            gameObject.SetActive(false);
        }
    }
}
