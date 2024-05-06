using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class javi_muere : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arma")
            gm.destruye(transform.gameObject.GetInstanceID());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arma")
            gm.destruye(transform.gameObject.GetInstanceID());
    }
}
