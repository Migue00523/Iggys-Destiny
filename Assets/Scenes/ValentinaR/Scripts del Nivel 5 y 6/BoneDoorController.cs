using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game.Level
{
    /// <summary>
    /// Igual que DoorController, pero en vez de requerir llaves
    /// se abre cuando todos los huesos han sido enterrados.
    /// </summary>
    public class BoneDoorController : MonoBehaviour
    {
        [SerializeField] private Collider2D blockingCollider;
        [SerializeField] private Sprite openSprite;
        [SerializeField] private Vector3 openLocalPosition;
        [SerializeField] private Vector3 openLocalScale = Vector3.one;

        [Tooltip("Dejar en 0 para contar automaticamente las lapidas activas de la escena.")]
        [SerializeField] private int requiredBones = 0;

        [SerializeField] private UnityEvent onLevelComplete;

        private SpriteRenderer _spriteRenderer;
        private int _bonesBuried;
        public bool IsOpen { get; private set; }
        private bool _completed;

        public int BonesBuried => _bonesBuried;
        public int RequiredBones => requiredBones;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (requiredBones <= 0)
            {
                requiredBones = FindObjectsByType<Grave>(FindObjectsSortMode.None).Length;
            }
        }

        /// <summary>
        /// Lo llama cada lapida cuando recibe su hueso.
        /// </summary>
        public void NotifyBoneBuried()
        {
            _bonesBuried++;

            if (_bonesBuried >= requiredBones)
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
