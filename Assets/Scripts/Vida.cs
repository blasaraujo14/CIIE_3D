using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Vida : MonoBehaviour
{
    public float vidaInicial;
    public float vidaActual;
    public UnityEvent eventoMorir;
    // Start is called before the first frame update
    void Start()
    {
        vidaActual = vidaInicial;
    }

    public void CausarDano(float cuanto)
    {
        vidaActual -= cuanto;
        if (vidaActual <= 0)
        {
            print("Muerto!!! ->" + gameObject.name);
            eventoMorir.Invoke();
        }
    }
}
