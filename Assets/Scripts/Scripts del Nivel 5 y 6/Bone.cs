using UnityEngine;
using Game.Player;

namespace Game.Level
{
    /// <summary>
    /// Hueso recogible. El jugador lo levanta al tocarlo y lo lleva encima
    /// hasta una lapida para enterrarlo.
    /// El collider de este objeto debe estar marcado como "Is Trigger".
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Bone : MonoBehaviour
    {
        [Header("Aspecto mientras lo carga el jugador")]
        [SerializeField] private Vector3 carriedLocalPosition = Vector3.zero;
        [SerializeField] private Vector3 carriedLocalEuler = Vector3.zero;
        [SerializeField] private Vector3 carriedLocalScale = Vector3.one;

        [Header("Aspecto al ser enterrado")]
        [Tooltip("Si esta activo, el hueso desaparece al enterrarse.")]
        [SerializeField] private bool hideWhenBuried = true;
        [Tooltip("Solo se usa si hideWhenBuried esta desactivado.")]
        [SerializeField] private Sprite buriedSprite;

        private Collider2D _collider;
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        public bool IsCarried { get; private set; }
        public bool IsBuried { get; private set; }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsCarried || IsBuried) return;
            if (!other.CompareTag("Player")) return;

            var carrier = other.GetComponentInParent<BoneCarrier>();
            if (carrier == null) return;

            carrier.TryPickUp(this);
        }

        /// <summary>
        /// Lo llama BoneCarrier al recoger el hueso. No llamar directamente.
        /// </summary>
        public void AttachTo(Transform carryPoint)
        {
            IsCarried = true;

            if (_collider != null) _collider.enabled = false;
            if (_rb != null) _rb.simulated = false;

            transform.SetParent(carryPoint, false);
            transform.localPosition = carriedLocalPosition;
            transform.localRotation = Quaternion.Euler(carriedLocalEuler);
            transform.localScale = carriedLocalScale;
        }

        /// <summary>
        /// Lo llama Grave al enterrar el hueso. No llamar directamente.
        /// </summary>
        public void Bury(Transform boneSlot)
        {
            IsCarried = false;
            IsBuried = true;

            if (_collider != null) _collider.enabled = false;
            if (_rb != null) _rb.simulated = false;

            if (hideWhenBuried)
            {
                transform.SetParent(null, true);
                gameObject.SetActive(false);
                return;
            }

            if (_spriteRenderer != null && buriedSprite != null)
            {
                _spriteRenderer.sprite = buriedSprite;
            }

            if (boneSlot != null)
            {
                transform.SetParent(boneSlot, false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
            }
        }
    }
}
