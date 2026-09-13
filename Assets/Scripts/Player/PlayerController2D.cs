using UnityEngine;

namespace Game.Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController2D : MonoBehaviour {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Camera boundsCamera;
        [SerializeField] private TouchJoystick touchJoystick;
        [SerializeField] private TouchJumpButton touchJumpButton;

        [Header("Sprite")]
        [Tooltip("Contenedor de todos los sprites del jugador (GraficosIguana). " +
                 "Se voltea entero para que las extremidades no se despeguen. " +
                 "Si se deja vacio se busca automaticamente.")]
        [SerializeField] private Transform graphicsRoot;

        private Rigidbody2D _rb;
        private float _moveInput;
        private bool _jumpRequested;
        private bool _isGrounded;
        private float _colliderRadius;
        private int _facing = 1; // 1 = derecha, -1 = izquierda
        private Vector3 _graphicsBaseScale = Vector3.one;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _rb.constraints |= RigidbodyConstraints2D.FreezeRotation;

            if (boundsCamera == null)
                 boundsCamera = Camera.main;
         

            ResolveGraphicsRoot();

            var col = GetComponent<Collider2D>();

            _colliderRadius = col != null? Mathf.Max(col.bounds.extents.x, col.bounds.extents.y)
                : 0.5f;
            Physics2D.gravity = new Vector2(0, -9.81f); // no borrar
        }

        private void ResolveGraphicsRoot() {
            if (graphicsRoot == null) {
                // Busca el contenedor que agrupa los sprites (el padre del primer SpriteRenderer).
                var sr = GetComponentInChildren<SpriteRenderer>();
                if (sr != null) {
                    graphicsRoot = (sr.transform.parent != null && sr.transform.parent != transform)
                        ? sr.transform.parent
                        : sr.transform;
                }
            }

            if (graphicsRoot == null) {
                Debug.LogWarning($"{name}: no se encontro el contenedor de graficos. El volteo de sprite no funcionara.", this);
                return;
            }

            // Guarda la escala original para no perderla al voltear.
            _graphicsBaseScale = graphicsRoot.localScale;
            _graphicsBaseScale.x = Mathf.Abs(_graphicsBaseScale.x);

            ApplyFacing();
        }

        private void Update() {
            float touchX = touchJoystick != null ? touchJoystick.Direction.x : 0f;
            _moveInput = Mathf.Abs(touchX) > 0.01f ? touchX : Input.GetAxisRaw("Horizontal");

            UpdateFacing();

            bool touchJump = touchJumpButton != null && touchJumpButton.JumpPressed;
            if (touchJump) {
                touchJumpButton.ConsumeJump();
            }

            if ((Input.GetButtonDown("Jump") || touchJump) && _isGrounded) {
                _jumpRequested = true;
            }
        }

        private void UpdateFacing() {
            if (Mathf.Abs(_moveInput) <= 0.01f) return;

            int dir = _moveInput > 0f ? 1 : -1;
            if (dir == _facing) return;

            _facing = dir;
            ApplyFacing();
        }

        private void ApplyFacing() {
            if (graphicsRoot == null) return;

            graphicsRoot.localScale = new Vector3(
                _graphicsBaseScale.x * _facing,
                _graphicsBaseScale.y,
                _graphicsBaseScale.z);
        }

        private void FixedUpdate() {
            _isGrounded = groundCheck != null &&
                Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (_jumpRequested) {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
                _jumpRequested = false;
            }

            _rb.linearVelocity = new Vector2(_moveInput * moveSpeed, _rb.linearVelocity.y);
        }

        private void LateUpdate() {
            if (boundsCamera == null || !boundsCamera.orthographic) return;

            float halfHeight = boundsCamera.orthographicSize;
            float halfWidth = halfHeight * boundsCamera.aspect;
            Vector2 camCenter = boundsCamera.transform.position;

            float minX = camCenter.x - halfWidth + _colliderRadius;
            float maxX = camCenter.x + halfWidth - _colliderRadius;
            float minY = camCenter.y - halfHeight + _colliderRadius;
            float maxY = camCenter.y + halfHeight - _colliderRadius;

            Vector2 pos = _rb.position;
            float clampedX = Mathf.Clamp(pos.x, minX, maxX);
            float clampedY = Mathf.Clamp(pos.y, minY, maxY);

            if (Mathf.Approximately(clampedX, pos.x) && Mathf.Approximately(clampedY, pos.y)) return;

            Vector2 vel = _rb.linearVelocity;
            if (!Mathf.Approximately(clampedX, pos.x)) vel.x = 0f;
            if (!Mathf.Approximately(clampedY, pos.y)) vel.y = 0f;
            _rb.linearVelocity = vel;
            _rb.position = new Vector2(clampedX, clampedY);
        }
    }
}