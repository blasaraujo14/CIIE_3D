using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asuna : MonoBehaviour
{
    Rigidbody cuerpo;
    public float vel;
    [SerializeField] Transform camara;
    public float ejeY;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        vel = 20f;
        cuerpo = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetBool("parado", true);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("corre", true);
            vel = 50f;
        }

        else
        {
            animator.SetBool("corre", false);
            vel = 20f;
        }
        transform.rotation = h != 0 || v != 0 ? Quaternion.Euler(transform.rotation.eulerAngles.x, camara.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z) : transform.rotation;
        cuerpo.Move((transform.forward * v + transform.right * h) * Time.deltaTime * vel + transform.position, transform.rotation);
        animator.SetBool("parado", h == 0 && v == 0);

        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetTrigger("salto");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        //other.gameObject.SetActive(false);
    }
}

