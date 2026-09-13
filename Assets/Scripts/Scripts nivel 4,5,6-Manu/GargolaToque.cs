using Game.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GargolaToque : MonoBehaviour
{
    [SerializeField] private DoorController tumba;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);

            if (toque.phase == TouchPhase.Began)
            {
                Vector3 posicionToque = Camera.main.ScreenToWorldPoint(toque.position);
                posicionToque.z = 0f;

                Collider2D objetoTocado = Physics2D.OverlapPoint(posicionToque);

                if (objetoTocado != null && objetoTocado.gameObject == gameObject)
                {
                    tumba.Open();
                }
            }
        }
    }
}