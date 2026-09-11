using UnityEngine;
using UnityEngine.Events;
using Game.Level;

namespace Game.Player
{
    /// <summary>
    /// Se agrega al jugador COMO COMPONENTE NUEVO.
    /// No requiere ningun cambio en PlayerController2D.
    /// Guarda que hueso lleva encima el jugador y donde se posiciona visualmente.
    /// </summary>
    public class BoneCarrier : MonoBehaviour
    {
        [Tooltip("GameObject vacio hijo del jugador donde se coloca el hueso. " +
                 "Si se deja vacio se usa el transform del jugador.")]
        [SerializeField] private Transform carryPoint;

        [Header("Eventos opcionales (sonido, particulas, etc.)")]
        [SerializeField] private UnityEvent onBonePickedUp;
        [SerializeField] private UnityEvent onBoneDelivered;

        private Bone _carriedBone;

        public bool IsCarryingBone => _carriedBone != null;
        public Bone CarriedBone => _carriedBone;

        private void Awake()
        {
            if (carryPoint == null)
            {
                carryPoint = transform;
            }
        }

        /// <summary>
        /// Lo llama Bone cuando el jugador lo toca.
        /// Devuelve false si el jugador ya lleva un hueso encima.
        /// </summary>
        public bool TryPickUp(Bone bone)
        {
            if (bone == null) return false;
            if (IsCarryingBone) return false;
            if (bone.IsCarried || bone.IsBuried) return false;

            _carriedBone = bone;
            bone.AttachTo(carryPoint);
            onBonePickedUp?.Invoke();
            return true;
        }

        /// <summary>
        /// Lo llama Grave al recibir el hueso. Suelta la referencia y devuelve el hueso.
        /// </summary>
        public Bone TakeCarriedBone()
        {
            var bone = _carriedBone;
            _carriedBone = null;

            if (bone != null)
            {
                onBoneDelivered?.Invoke();
            }

            return bone;
        }
    }
}
