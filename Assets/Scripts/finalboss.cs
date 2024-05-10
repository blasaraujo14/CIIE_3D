using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]


public class finalboss : EnemigoFinal
{
    private NavMeshAgent agente;
    public Animator animaciones;
    public bool sensor;
    private bool sensorAccion;
    public static float dano = 3;
    javi_muere javi;
    void Awake()
    {

        agente = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        base.Start();
        sensorAccion = false;
        sensor = false;
        Invoke("getJavi", 1);
    }

    private void getJavi()
    {
        javi = gameObject.GetComponent<javi_muere>();
        javi.vida = 300;
    }
    private void Update()
    {
        base.Update();
        if (javi != null && javi.vida < 150) ActivacionDelSensor1();
        OnSensor();
    }
    public override void EstadoIdle()
    {
        base.EstadoIdle();
        animaciones.SetFloat("velocidad", 0);
        animaciones.SetBool("atacando", false);
        agente.SetDestination(transform.position);
    }
    public override void EstadoSeguir()
    {
        base.EstadoSeguir();
        animaciones.SetFloat("velocidad", 1);
        animaciones.SetBool("atacando", false);
        agente.SetDestination(target.position);
    }
    public override void EstadoAtacar()
    {
        base.EstadoAtacar();
        animaciones.SetFloat("velocidad", 0);
        animaciones.SetBool("atacando", true);
        agente.SetDestination(transform.position);
        transform.LookAt(target, Vector3.up);

        /*
        if (Personaje.singleton.vida.vidaActual <= 0)
        {
            animaciones.SetBool("atacando", false);
        }*/
    }
    public override void EstadoMuerto()
    {
        base.EstadoMuerto();
        animaciones.SetBool("vivo", false);
        animaciones.SetBool("atacando", false);
        agente.enabled = false;
    }
    [ContextMenu("EstadoAtaqueSalto")]
    public override void EstadoAtaqueSalto()
    {
        base.EstadoAtaqueSalto();
        animaciones.SetBool("jump", true);
        CambiarEstado(EstadosEF.jump);


    }
    [ContextMenu("Matar")]
    public void Matar()
    {
        CambiarEstado(EstadosEF.muerto);
    }

    public void DesactivarAtaqueSalto()
    {
        animaciones.SetBool("jump", false);
        base.ataqueSalto = false;
        CambiarEstado(EstadosEF.idle);
    }
    public void ActivacionDelSensor1()
    {
        sensorAccion = true;
    }
    public void OnSensor()
    {
        if (!sensor && sensorAccion)
        {
            sensor = true;
            sensorAccion = false;
            AccionSensor();
        }
    }

    public void AccionSensor()
    {
        this.transform.Find("Sensor").gameObject.SetActive(true);
        this.transform.Find("Sensor").gameObject.GetComponent<Sensor>().enabled = true;
        this.transform.Find("Sensor").gameObject.GetComponent<Sensor>().bucle = true;
    }

}
