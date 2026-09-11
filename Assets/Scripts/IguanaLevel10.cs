using UnityEngine;

namespace Game.Player
{
    public class IguanaLevel10 : MonoBehaviour
    {
        [Header("Platforms")]
        [SerializeField] private Transform[] platforms;

        [Header("Shake")]
        [SerializeField] private float shakeThreshold = 2.5f;
        [SerializeField] private float shakeCooldown = 0.5f;

        private int currentPlatform = 0;
        private float lastShakeTime;

        private void Update()
        {
            DetectShake();
        }

        private void DetectShake()
        {
            Vector3 acceleration = Input.acceleration;

            if (acceleration.magnitude > shakeThreshold &&
                Time.time > lastShakeTime + shakeCooldown)
            {
                TeleportToNextPlatform();

                lastShakeTime = Time.time;
            }
        }

        private void TeleportToNextPlatform()
        {
            if (platforms == null || platforms.Length == 0)
                return;

            currentPlatform++;

            if (currentPlatform >= platforms.Length)
                currentPlatform = platforms.Length - 1;

            transform.position = platforms[currentPlatform].position;
        }
    }
}