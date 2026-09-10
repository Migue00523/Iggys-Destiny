using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController2D : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Camera boundsCamera;
        [SerializeField] private TouchJoystick touchJoystick;
        [SerializeField] private TouchJumpButton touchJumpButton;

        private Rigidbody2D _rb;
        private float _moveInput;
        private bool _jumpRequested;
        private bool _isGrounded;
        private float _colliderRadius;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            if (boundsCamera == null)
            {
                boundsCamera = Camera.main;
            }

            var col = GetComponent<Collider2D>();
            _colliderRadius = col != null ? Mathf.Max(col.bounds.extents.x, col.bounds.extents.y) : 0.5f;
        }

        private void Update()
        {
            float touchX = touchJoystick != null ? touchJoystick.Direction.x : 0f;
            _moveInput = Mathf.Abs(touchX) > 0.01f ? touchX : Input.GetAxisRaw("Horizontal");

            bool touchJump = touchJumpButton != null && touchJumpButton.JumpPressed;
            if (touchJump)
            {
                touchJumpButton.ConsumeJump();
            }

            if ((Input.GetButtonDown("Jump") || touchJump) && _isGrounded)
            {
                _jumpRequested = true;
            }
        }

        private void FixedUpdate()
        {
            _isGrounded = groundCheck != null &&
                Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (_jumpRequested)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
                _jumpRequested = false;
            }

            _rb.linearVelocity = new Vector2(_moveInput * moveSpeed, _rb.linearVelocity.y);
        }

        private void LateUpdate()
        {
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
