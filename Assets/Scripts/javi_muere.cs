using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class javi_muere : MonoBehaviour
{
    GameManager gm;
    public int vida;
    AudioSource ai;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        //vida = gameObject.name == "enemigo1(Clone)" ? 100 : gameObject.name == "enemigo2(Clone)" ? 50 : 300;
        vida = gameObject.name == "enemigo1(Clone)" ? 100 : 50;
        ai = gameObject.GetComponent<AudioSource>();
        ai.clip = (AudioClip)Resources.Load("Audios/Sfx/Robots/DronHit");
        ai.volume = PlayerPrefs.GetFloat("SFX");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arma")
        {
            ai.Play();
            if (other.gameObject.name == "nudillo") vida -= 30;
            else vida -= 10;
            if (vida <= 0) gm.destruye(transform.gameObject);
        }
    }
}
