using UnityEngine;
using UnityEngine.UI;

public class GuardadoNiveles : MonoBehaviour
{
    public int nivel;
    public Button btn;
    public Text txt;

    void Start()
    {
        txt.text = nivel.ToString();
        int NivelDesbloqueado = 2;

        if (nivel <= NivelDesbloqueado+1)
        {
            btn.interactable = true;
        }
        else
        {
            btn.interactable = false;
        }
    }

    
    void Update()
    {
        
    }
}
