using UnityEngine;
using UnityEngine.SceneManagement;

public class AguilaController : MonoBehaviour
{
    [Header("References")]
    public MicrophoneDetector microphoneDetector;

    [Header("Attack Settings")]
    public float attackDelay = 1f;

    private bool isAttacking = false;

    void Update()
    {
        if (microphoneDetector == null)
            return;

        if (microphoneDetector.IsMakingNoise && !isAttacking)
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
        isAttacking = true;

        Debug.Log("The eagle detected noise!");

        Invoke(nameof(RestartLevel), attackDelay);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}
