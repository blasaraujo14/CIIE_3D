using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class animacionJefeFinal : MonoBehaviour
{
    GameObject maquina;
    GameObject player;
    public GameObject jefe;
    GameObject explosion;
    GameObject sonidoExplosion;
    AudioSource bum;
    GameObject camara;
    // Start is called before the first frame update
    void Start()
    {
        maquina = GameObject.FindGameObjectWithTag("Destruye");
        player = GameObject.FindGameObjectWithTag("Player");
        explosion = (GameObject)Resources.Load("BigExplosion");
        sonidoExplosion = new GameObject();
        sonidoExplosion.AddComponent<AudioSource>();
        bum = sonidoExplosion.GetComponent<AudioSource>();
        bum.playOnAwake = false;
        bum.clip = (AudioClip)Resources.Load("Audios/Sfx/explosion");
        //player.transform.LookAt(maquina.transform);
        camara = GameObject.FindGameObjectWithTag("MainCamera");
        gameObject.GetComponent<Canvas>().worldCamera = camara.GetComponent<Camera>();
        Invoke("Explota", 2f);
        jefe.transform.position = maquina.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Explota()
    {
        Destroy(sonidoExplosion, 1.5f);
        Destroy(Instantiate(explosion), 1f);
        maquina.SetActive(false);
        jefe.SetActive(true);
        Invoke("Peta", 0.5f);
    }
    
    private void Peta()
    {
        player.SetActive(true);
        camara.GetComponent<CameraOrbit>().enabled = true;
        Destroy(gameObject);
    }
}
