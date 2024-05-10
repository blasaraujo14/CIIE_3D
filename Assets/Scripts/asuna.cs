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
    Transform camara;
    public float ejeY;
    Animator animator;
    public float velosidat;
    public float fuerzaSalto;
    public bool salto;
    bool rifle;
    bool pistola;
    float deltaDisparo;
    float CADENCIARIFLE = 0.2f;
    [SerializeField] GameObject rifleObj;
    [SerializeField] GameObject pistolaObj;
    [SerializeField] GameObject nudillo;
    public int municion;
    GameObject bala;
    GameObject puntaBala;
    Text municionText;
    Image vidaImg;
    public bool golpe;
    public AudioSource pasos;
    AudioClip ai;
    AudioSource disparoRifle;
    AudioSource disparoPistola;
    AudioSource nudilloSonido;
    int vida;
    float invencibleTimer;

    // Start is called before the first frame update
    void Start()
    {
        vida = 100;
        MonoBehaviour[] lista = GetComponentsInChildren<MonoBehaviour>();

        //colliderSuelo = GameObject.Find("Collider_suelo").GetComponent<Collider>();
        municion = 0;
        vel = 20f;
        camara = GameObject.FindGameObjectWithTag("MainCamera").transform;
        fuerzaSalto = 30f;
        salto = false;
        cuerpo = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetBool("parado", true);
        //rifleObj = transform.Find("mixamorig:Hips").gameObject;
        rifleObj.SetActive(false);
        pistolaObj.SetActive(false);
        deltaDisparo = 0;
        bala = (GameObject)Resources.Load("Bala");
        puntaBala = rifleObj.transform.Find("PuntaRifle").gameObject;
        municionText = GameObject.Find("currAmo").GetComponent<Text>();
        municionText.text = "";
        vidaImg = GameObject.Find("VidaImg").GetComponent<Image>();
        vidaImg.fillAmount = 1;
        golpe = false;
        pasos = transform.GetComponent<AudioSource>();
        pasos.clip = (AudioClip)Resources.LoadAll("Audios/Sfx/Pasos/" + SceneManager.GetActiveScene().name)[UnityEngine.Random.Range(0, 5)];
        pasos.volume = PlayerPrefs.GetFloat("SFX") * 0.6f;
        disparoRifle = rifleObj.GetComponent<AudioSource>();
        disparoRifle.clip = (AudioClip)Resources.Load("Audios/Sfx/Armas/rifle");
        disparoRifle.volume = PlayerPrefs.GetFloat("SFX");
        disparoPistola = pistolaObj.GetComponent<AudioSource>();
        disparoPistola.clip = (AudioClip)Resources.Load("Audios/Sfx/Armas/pistol");
        disparoPistola.volume = PlayerPrefs.GetFloat("SFX");
        nudilloSonido = nudillo.GetComponent<AudioSource>();
        nudilloSonido.clip = (AudioClip)Resources.Load("Audios/Sfx/Armas/nudillo");
        nudilloSonido.volume = PlayerPrefs.GetFloat("SFX");
        ai = (AudioClip)Resources.Load("Audios/Sfx/Personaje/Hit");
        invencibleTimer = 0.5f;
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
        invencibleTimer -= Time.deltaTime;
        animator.ResetTrigger("golpe");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Input.GetMouseButton(0))
        {
            if (rifle)
            {
                disparoRifle.Play();
                dispara(CADENCIARIFLE);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (pistola)
            {
                disparoPistola.Play();
                dispara(0);
            }
            else if (!rifle)
            {
                golpeEvent(0);
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
            if(!pasos.isPlaying)
            {
                pasos.Play();
            }
        }
        else pasos.Stop();
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
        if (golpe)
        {
            animator.SetTrigger("golpe");
            //nudillo.SetActive(true);
        }
    }

    private void OnDisable()
    {
        pasos.Stop();
    }

    private void OnEnable()
    {
        if (pasos != null) pasos.volume = PlayerPrefs.GetFloat("SFX") * 0.6f;
        if(disparoRifle != null) disparoRifle.volume = PlayerPrefs.GetFloat("SFX");
        if(disparoPistola != null) disparoPistola.volume = PlayerPrefs.GetFloat("SFX");
        if (nudilloSonido != null) nudilloSonido.volume = PlayerPrefs.GetFloat("SFX");
    }

    public void cambiaArma(String arma)
    {
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

    public void golpeEvent(int inicio)
    {
        golpe = inicio == 0;
        nudillo.SetActive(inicio == 0);
        if (inicio == 0)
        {
            nudilloSonido.Play();
            animator.SetTrigger("golpe");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        bool dano = false;
        if (invencibleTimer < 0)
        {
            if (other.gameObject.tag == "manotazo")
            {
                vida -= 10;
                dano = true;
            }
            else if (other.gameObject.tag == "manotazoBoss")
            {
                vida -= 20;
                dano = true;
            }
            else if (other.gameObject.tag == "Sensor")
            {
                vida -= 30;
                dano = true;
            }
            if (dano)
            {
                AudioClip preClip = pasos.clip;
                pasos.clip = ai;
                pasos.Play();
                vidaImg.fillAmount = (float)(vida / 100f);
                invencibleTimer = 0.5f;
                ai = preClip;
                Invoke("Paseo", 0.3f);
            }
            if (vida <= 0)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Muerte();
                enabled = false;
            }
        }
    }

    public void Paseo()
    {
        AudioClip preClip = pasos.clip;
        pasos.clip = ai;
        ai = preClip;
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}