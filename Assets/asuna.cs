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
    public float velosidat;
    public float fuerzaSalto;
    public bool salto;
    bool rifle;
    bool pistola;
    float tiempo;
    float deltaDisparo;
    float CADENCIARIFLE = 0.2f;
    [SerializeField] GameObject rifleObj;
    GameObject bala;
    GameObject puntaBala;

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
        //rifleObj = transform.Find("mixamorig:Hips").gameObject;
        rifleObj.SetActive(false);
        pistola = false;
        tiempo = 10;
        deltaDisparo = 0;
        bala = (GameObject)Resources.Load("Bala");
        puntaBala = rifleObj.transform.Find("PuntaRifle").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        deltaDisparo += Time.deltaTime;
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
        if (Input.GetMouseButton(0))
        {
            if (rifle)
            {
                dispara();
            }
        }
        if(rifle)
        {
            tiempo -= Time.deltaTime;
        }
        if (tiempo < 0)
        {
            animator.SetBool("rifle", false);
            animator.SetTrigger("arma");
            rifleObj.SetActive(false);
            rifle = false;
            tiempo = 10;

        }
        if (Time.frameCount >= 300)Profiler.EndSample();
    }

    public void cambiaArma(String arma)
    {
        rifle = true;
        animator.SetBool("rifle", true);
        animator.SetTrigger("arma");
        //animator.SetTrigger("rifle");
        rifleObj.SetActive(true);
        tiempo = 10;
    }

    void dispara()
    {
        if (deltaDisparo > CADENCIARIFLE)
        {
            GameObject.Instantiate(bala, puntaBala.transform.position, transform.rotation);
            deltaDisparo = 0;
        }
    }
}