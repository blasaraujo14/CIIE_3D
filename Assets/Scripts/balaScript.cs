using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balaScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward*60, ForceMode.Impulse);
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name + " " + other.gameObject.transform.position);
        if(other.gameObject.tag != "Ignore")
        {
            Destroy(gameObject, 1);
            gameObject.SetActive(false);
        }
    }
}
