using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    Rigidbody cuerpo;
    public float vel;
    [SerializeField] Transform camara;

    // Start is called before the first frame update
    void Start()
    {
        vel = 50f;
        cuerpo = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //cuerpo.AddForce(new Vector3(h, v, 1)*Time.deltaTime*vel);
        //cuerpo.Move(new Vector3(h,0,v)*Time.deltaTime*vel+transform.position, transform.rotation);
        //cuerpo.Move(Vector3.Dot(Vector3.forward, new Vector3(h, 0, v)) * Time.deltaTime * vel + transform.position, transform.rotation);
        //cuerpo.transform.position = new Vector3(h, 0, v).normalized;
        //cuerpo.MoveRotation(Quaternion.RotateTowards(Quaternion.Euler(my * 360 * sensibilidad_camara, mx * 360 * sensibilidad_camara, 0), transform.rotation, 1));
        //cuerpo.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(my * 360 * sensibilidad_camara, mx * 360 * sensibilidad_camara, 0), 1);
    }
}
