using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animacionJefeFinal : MonoBehaviour
{
    GameObject maquina;
    GameObject player;
    GameObject jefe;
    // Start is called before the first frame update
    void Start()
    {
        maquina = GameObject.FindGameObjectWithTag("Destruye");
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.LookAt(maquina.transform);
        gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Invoke("Explota", 2f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Explota()
    {
        //particulillas
        maquina.SetActive(false);
        GameObject.FindGameObjectWithTag("Enemigo").SetActive(true);
    }
}
