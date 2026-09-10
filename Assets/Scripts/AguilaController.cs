using UnityEngine;
using UnityEngine.SceneManagement;

public class EagleController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MicrophoneDetector microphoneDetector;
    [SerializeField] private Transform player;

    [Header("Patrol")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float changeTargetTime = 3f;

    [Header("Attack")]
    [SerializeField] private float attackSpeed = 8f;
    [SerializeField] private float attackDistance = 0.5f;

    private Vector3 patrolTarget;
    private float targetTimer;
    private bool isAttacking = false;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        ChooseNewPatrolTarget();
    }

    private void Update()
    {
        if (microphoneDetector == null)
            return;

        // Detect noise immediately
        if (microphoneDetector.IsMakingNoise && !isAttacking)
        {
            StartAttack();
        }

        if (isAttacking)
        {
            MoveTowardsPlayer();
        }
        else
        {
            Patrol();
        }
    }

    // =========================
    // PATROL
    // =========================

    private void Patrol()
    {
        targetTimer -= Time.deltaTime;

        if (targetTimer <= 0f)
        {
            ChooseNewPatrolTarget();
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            patrolTarget,
            patrolSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            ChooseNewPatrolTarget();
        }
    }

    private void ChooseNewPatrolTarget()
    {
        targetTimer = changeTargetTime;

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, 0)
        );

        Vector3 topRight = mainCamera.ViewportToWorldPoint(
            new Vector3(1, 1, 0)
        );

        float randomX = Random.Range(
            bottomLeft.x,
            topRight.x
        );

        float randomY = Random.Range(
            bottomLeft.y,
            topRight.y
        );

        patrolTarget = new Vector3(
            randomX,
            randomY,
            transform.position.z
        );
    }

    // =========================
    // ATTACK
    // =========================

    private void StartAttack()
    {
        isAttacking = true;

        Debug.Log("¡El águila detectó ruido!");
    }

    private void MoveTowardsPlayer()
    {
        if (player == null)
            return;

        // Move directly towards the iguana
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            attackSpeed * Time.deltaTime
        );

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        // Restart immediately when the eagle reaches the player
        if (distance <= attackDistance)
        {
            RestartLevel();
        }
    }

    // =========================
    // RESTART
    // =========================

    private void RestartLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}