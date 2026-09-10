using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game.Level
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private Collider2D blockingCollider;
        [SerializeField] private Sprite openSprite;
        [SerializeField] private Vector3 openLocalPosition;
        [SerializeField] private Vector3 openLocalScale = Vector3.one;
        [SerializeField] private int requiredKeys = 0;
        [SerializeField] private UnityEvent onLevelComplete;

        private SpriteRenderer _spriteRenderer;
        private int _keysCollected;
        public bool IsOpen { get; private set; }
        private bool _completed;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void CollectKey()
        {
            _keysCollected++;
            if (_keysCollected >= requiredKeys)
            {
                Open();
            }
        }

        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;

            if (blockingCollider != null)
            {
                blockingCollider.enabled = false;
            }

            if (_spriteRenderer != null && openSprite != null)
            {
                _spriteRenderer.sprite = openSprite;
                transform.position = openLocalPosition;
                transform.localScale = openLocalScale;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_completed || !IsOpen || !other.CompareTag("Player")) return;

            _completed = true;
            onLevelComplete?.Invoke();

            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextIndex);
            }
            else
            {
                Debug.Log("Nivel completado. No hay un siguiente nivel configurado en Build Settings.");
            }
        }
    }
}
