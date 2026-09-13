using UnityEngine;

namespace Game.Level
{
    /// <summary>
    /// Plataforma que se mueve de arriba a abajo (o en cualquier direccion) de forma continua.
    /// Usa un Rigidbody2D Kinematic movido con MovePosition, lo cual hace que el motor de
    /// fisica transmita el movimiento al jugador de forma natural por friccion/contacto:
    /// este script NUNCA toca al jugador ni a su Rigidbody2D directamente.
    ///
    /// REQUISITOS EN EL EDITOR:
    /// 1. El GameObject de la plataforma necesita un Rigidbody2D.
    ///    - Body Type: Kinematic  (este script lo fuerza en Awake por si acaso)
    /// 2. El GameObject necesita un Collider2D (no trigger) para que el jugador
    ///    pueda pararse encima.
    /// 3. El jugador necesita algo de friccion en su Physics Material 2D
    ///    (o en el material de la plataforma) para que "se pegue" al moverse.
    ///    Sin friccion, el jugador se resbalara ligeramente al subir/bajar.
    /// 4. El collider del jugador NO debe estar en modo Trigger.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovingPlatform2D : MonoBehaviour
    {
        [Header("Movimiento")]
        [Tooltip("Distancia que recorre la plataforma desde su posicion inicial, en unidades del mundo.")]
        [SerializeField] private float distance = 3f;

        [Tooltip("Direccion del movimiento. Se normaliza automaticamente. Para arriba/abajo dejar en (0,1).")]
        [SerializeField] private Vector2 direction = Vector2.up;

        [Tooltip("Velocidad de desplazamiento en unidades por segundo.")]
        [SerializeField] private float speed = 2f;

        [Tooltip("Si esta activo, la plataforma hace pausa breve en cada extremo antes de regresar.")]
        [SerializeField] private float pauseAtEnds = 0f;

        [Header("Offset inicial (opcional)")]
        [Tooltip("Desplaza el punto de partida del ciclo, en unidades de tiempo (0 a 1). " +
                 "Util para desincronizar varias plataformas que comparten timing.")]
        [Range(0f, 1f)]
        [SerializeField] private float cycleOffset = 0f;

        private Rigidbody2D _rb;
        private Vector2 _startPos;
        private Vector2 _endPos;
        private float _journeyLength;
        private float _elapsedAtStart;
        private float _pauseTimer;
        private bool _movingForward = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            // Fuerza el tipo correcto aunque se haya dejado mal configurado en el Inspector.
            _rb.bodyType = RigidbodyType2D.Kinematic;

            // Kinematic con interpolacion evita el jitter visual del jugador al ir montado encima.
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.up;

            _startPos = _rb.position;
            _endPos = _startPos + direction * distance;
            _journeyLength = Vector2.Distance(_startPos, _endPos);

            // Aplica el offset de ciclo para desincronizar si se desea.
            _elapsedAtStart = cycleOffset * (_journeyLength / Mathf.Max(speed, 0.0001f));
        }

        private void FixedUpdate()
        {
            if (_journeyLength <= 0f || speed <= 0f) return;

            if (_pauseTimer > 0f)
            {
                _pauseTimer -= Time.fixedDeltaTime;
                return;
            }

            float step = speed * Time.fixedDeltaTime;
            Vector2 target = _movingForward ? _endPos : _startPos;
            Vector2 currentPos = _rb.position;

            Vector2 newPos = Vector2.MoveTowards(currentPos, target, step);
            _rb.MovePosition(newPos);

            // Llego al extremo: invierte direccion (con pausa opcional).
            if (Vector2.Distance(newPos, target) <= 0.0001f)
            {
                _movingForward = !_movingForward;
                if (pauseAtEnds > 0f)
                {
                    _pauseTimer = pauseAtEnds;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Visualiza el recorrido en el editor sin necesidad de darle Play.
            Vector3 start = Application.isPlaying ? (Vector3)_startPos : transform.position;
            Vector3 dir = direction.sqrMagnitude > 0.0001f ? (Vector3)direction.normalized : Vector3.up;
            Vector3 end = start + dir * distance;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawWireSphere(start, 0.15f);
            Gizmos.DrawWireSphere(end, 0.15f);
        }
    }
}
