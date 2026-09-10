using UnityEngine;
using UnityEngine.UI;

namespace Game.Level
{
    [RequireComponent(typeof(Button))]
    public class IntroPanelController : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("[IntroPanel] Awake - panel activo, esperando click");
            Time.timeScale = 0f;
            GetComponent<Button>().onClick.AddListener(Dismiss);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("[IntroPanel] Input.GetMouseButtonDown detectado en " + Input.mousePosition);
            }
            if (Input.anyKeyDown)
            {
                Debug.Log("[IntroPanel] Input.anyKeyDown detectado");
            }
        }

        private void Dismiss()
        {
            Debug.Log("[IntroPanel] Click recibido, cerrando panel");
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
