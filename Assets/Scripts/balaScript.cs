using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balaScript : MonoBehaviour
{
    float timer;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward*60, ForceMode.Impulse);
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name + " " + other.gameObject.transform.position);
        if(other.gameObject.tag != "Ignore")
        {
            Debug.Log(other.gameObject.name + " " + other.gameObject.tag);
            Destroy(gameObject, 1);
            gameObject.SetActive(false);
            timer = 1;
        }
    }
}
