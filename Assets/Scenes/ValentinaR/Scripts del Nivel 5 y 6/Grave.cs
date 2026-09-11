using UnityEngine;
using UnityEngine.Events;
using Game.Player;

namespace Game.Level
{
    /// <summary>
    /// Lapida. Recibe cualquier hueso, pero solo uno: una vez ocupada
    /// ya no acepta mas.
    /// El collider de este objeto debe estar marcado como "Is Trigger".
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Grave : MonoBehaviour
    {
        [Tooltip("Opcional. Punto donde se coloca el hueso enterrado si decides mostrarlo.")]
        [SerializeField] private Transform boneSlot;

        [Tooltip("Opcional. Sprite de la lapida ya ocupada.")]
        [SerializeField] private Sprite occupiedSprite;

        [Tooltip("Si se deja vacio, se busca automaticamente el baul de la escena.")]
        [SerializeField] private BoneDoorController door;

        [Header("Evento opcional (sonido, particulas, etc.)")]
        [SerializeField] private UnityEvent onBoneBuried;

        private SpriteRenderer _spriteRenderer;

        public bool IsOccupied { get; private set; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (door == null)
            {
                door = FindFirstObjectByType<BoneDoorController>();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryBury(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryBury(other);
        }

        private void TryBury(Collider2D other)
        {
            if (IsOccupied) return;
            if (!other.CompareTag("Player")) return;

            var carrier = other.GetComponentInParent<BoneCarrier>();
            if (carrier == null || !carrier.IsCarryingBone) return;

            var bone = carrier.TakeCarriedBone();
            if (bone == null) return;

            IsOccupied = true;
            bone.Bury(boneSlot);

            if (_spriteRenderer != null && occupiedSprite != null)
            {
                _spriteRenderer.sprite = occupiedSprite;
            }

            onBoneBuried?.Invoke();

            if (door != null)
            {
                door.NotifyBoneBuried();
            }
        }
    }
}
