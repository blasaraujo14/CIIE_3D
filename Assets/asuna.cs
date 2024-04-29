using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public class asuna : MonoBehaviour
{
    Rigidbody cuerpo;
    public float vel;
    [SerializeField] Transform camara;
    public float ejeY;
    Animator animator;
    float velocidadEn0;
    public float velosidat;
    public float fuerzaSalto;
    GameObject pieDerecho;
    public bool salto;
    Collider colliderSuelo;
    bool rifle;
    bool pistola;
    float tiempo;
    [SerializeField] GameObject rifleObj;

    // Start is called before the first frame update
    void Start()
    {
        MonoBehaviour[] lista = GetComponentsInChildren<MonoBehaviour>();

        //colliderSuelo = GameObject.Find("Collider_suelo").GetComponent<Collider>();
        vel = 20f;
        fuerzaSalto = 30f;
        salto = false;
        cuerpo = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetBool("parado", true);
        rifle = false;
        Debug.Log(transform.childCount);
        //rifleObj = transform.Find("mixamorig:Hips").gameObject;
        rifleObj.SetActive(false);
        pistola = false;
        tiempo = 3;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            if (p.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(p.name);
            }
        }
        */
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift) && salto)
        {
            animator.SetBool("corre", true);
            vel = 40f;
        }

        else if (salto)
        {
            animator.SetBool("corre", false);
            vel = 20f;
        }
        if (v < 0)
        {
            vel = (float)(vel * 0.5);
        }
        transform.rotation = h != 0 || v != 0 ? Quaternion.Euler(transform.rotation.eulerAngles.x, camara.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z) : transform.rotation;
        cuerpo.Move((transform.forward * v + transform.right * h) * Time.deltaTime * vel + transform.position, transform.rotation);
        //cuerpo.addForce((transform.forward * v + transform.right * h) * Time.deltaTime * vel, ForceMode.VelocityChange);
        animator.SetFloat("Direccion", v);
        animator.SetBool("parado", h == 0 && v == 0);


        if (Input.GetKey(KeyCode.Space) && salto)
        {
            salto = false;
            animator.SetTrigger("salto");
            cuerpo.AddForce(transform.up * fuerzaSalto, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            //animator.SetTrigger("Patada");
        }
        /*
        if(Input.GetKey(KeyCode.P))
        {
            animator.SetBool("rifle", rifle);
            rifle = !rifle;
        }
        */
        if(rifle)
        {
            tiempo -= Time.deltaTime;
        }
        if (tiempo < 0)
        {
            animator.SetBool("rifle", false);
            rifleObj.SetActive(false);
            tiempo = 3;

        }
        if (Time.frameCount >= 300)Profiler.EndSample();
    }

    public void cambiaArma(String arma)
    {
        rifle = true;
        animator.SetBool("rifle", true);
        rifleObj.SetActive(true);
        tiempo = 3;
    }
}