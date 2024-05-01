using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balaScript : MonoBehaviour
{
    float timer;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward*20, ForceMode.Impulse);
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject, 1);
        gameObject.SetActive(false);
        timer = 1;
    }
}
