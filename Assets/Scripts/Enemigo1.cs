using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]

public class Enemigo1 : Enemigo
{
    private NavMeshAgent agente;
    public Animator animaciones;
    public static float dano = 3;
    void Awake()
    {

        agente = GetComponent<NavMeshAgent>();
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
        }
        */
    }
    public override void EstadoMuerto()
    {
        base.EstadoMuerto();
        animaciones.SetBool("vivo", false);
        animaciones.SetBool("atacando", false);
        agente.enabled = false;
    }
    [ContextMenu("Matar")]
    public void Matar()
    {
        CambiarEstado(Estados.muerto);
    }

}
