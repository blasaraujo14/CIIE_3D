using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public static Personaje singleton;
    public Vida vida;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("manotazo"))
        {
            Personaje.singleton.vida.CausarDano(Enemigo1.dano);
        }
    }


}
