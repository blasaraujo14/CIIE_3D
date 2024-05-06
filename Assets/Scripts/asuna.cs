using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    bool pistolaCanShoot;
    float tiempoItem;
    float deltaDisparo;
    float CADENCIARIFLE = 0.2f;
    [SerializeField] GameObject rifleObj;
    [SerializeField] GameObject pistolaObj;
    public int municion;
    GameObject bala;
    GameObject puntaBala;
    public Text municionText;

    // Start is called before the first frame update
    void Start()
    {
        MonoBehaviour[] lista = GetComponentsInChildren<MonoBehaviour>();

        //colliderSuelo = GameObject.Find("Collider_suelo").GetComponent<Collider>();
        municion = 0;
        vel = 20f;
        fuerzaSalto = 30f;
        salto = false;
        cuerpo = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetBool("parado", true);
        pistolaCanShoot = true;
        //rifleObj = transform.Find("mixamorig:Hips").gameObject;
        rifleObj.SetActive(false);
        pistolaObj.SetActive(false);
        deltaDisparo = 0;
        bala = (GameObject)Resources.Load("Bala");
        puntaBala = rifleObj.transform.Find("PuntaRifle").gameObject;
        municionText.text = "";
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
        if (Input.GetMouseButton(0))
        {
            if (rifle)
            {
                dispara(CADENCIARIFLE);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (pistola)
            {
                dispara(0);
            }
        }
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
        Vector3 direction = camara.forward * v + camara.right * h;
        direction.Normalize();
        if (v!=0 || h != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
        //transform.rotation = h != 0 || v != 0 ? Quaternion.Euler(transform.rotation.eulerAngles.x, camara.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z) : transform.rotation;
        //cuerpo.Move((transform.forward * (v>0?v:-v) + transform.right * h) * Time.deltaTime * vel + transform.position, transform.rotation);
        cuerpo.Move(direction * Time.deltaTime * vel + transform.position, transform.rotation);
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
        if (municion == 0)
        {
            municionText.text = "";
            municion--;
            animator.SetBool("rifle", false);
            animator.SetBool("pistola", false);
            animator.SetTrigger("arma");
            rifleObj.SetActive(false);
            pistolaObj.SetActive(false);
            rifle = false;
            pistola = false;
        }
    }

    public void cambiaArma(String arma)
    {
        Debug.Log(arma);
        if (arma == "Rifle")
        {
            municion = 30;
            rifle = true;
            pistola = false;

            rifleObj.SetActive(true);
            pistolaObj.SetActive(false);
        }
        else if (arma == "Pistola")
        {
            municion = 16;
            pistola = true;
            rifle = false;

            pistolaObj.SetActive(true);
            rifleObj.SetActive(false);
        }
        municionText.text = municion.ToString();
        animator.SetTrigger("arma");
        animator.SetBool("rifle", rifle);
        animator.SetBool("pistola", pistola);
    }

    void dispara(float cadencia)
    {
        if (deltaDisparo > cadencia)
        {
            if (0 < municion--)
            {
                municionText.text = municion.ToString();
                transform.rotation = Quaternion.Euler(0, camara.rotation.eulerAngles.y, 0);
                //GameObject.Instantiate(bala, puntaBala.transform.position, transform.rotation);
                GameObject.Instantiate(bala, puntaBala.transform.position, camara.rotation*Quaternion.Euler(0,0,0));
                deltaDisparo = 0;
            }
        }
    }
}