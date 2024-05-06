using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider_suelo : MonoBehaviour
{
    asuna asunaScript;
    // Start is called before the first frame update
    void Start()
    {
        asunaScript = GetComponentInParent<asuna>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        asunaScript.salto = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        asunaScript.salto = true;
    }
}
