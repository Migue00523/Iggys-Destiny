using UnityEngine;
using UnityEngine.UI;

public class Llamado : MonoBehaviour
{
    public int nivel;
    public Button btn;
    public Text txt;

    void Start()
    {
       txt.text =  nivel.ToString();
        int NivelGuardado = PlayerPrefs.GetInt("Nivel");
        btn.interactable = NivelGuardado <= nivel + 1;
    }

 
}
