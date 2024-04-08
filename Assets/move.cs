using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    Rigidbody cuerpo;
    public float vel;
    [SerializeField] Transform camara;
    public float ejeY;

    // Start is called before the first frame update
    void Start()
    {
        vel = 30f;
        cuerpo = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.rotation = h != 0 || v != 0? Quaternion.Euler(transform.rotation.eulerAngles.x, camara.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z) : transform.rotation;
        cuerpo.Move(transform.forward * v * Time.deltaTime * vel + transform.right * h * Time.deltaTime * vel + transform.position, transform.rotation);

        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);   
        //other.gameObject.SetActive(false);
    }
}

