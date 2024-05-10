using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemigoFinal : MonoBehaviour
{
    public EstadosEF estado;
    public float distanciaSeguir;
    public float distanciaAtacar;
    public float distanciaEscapar;


    public bool autoseleccionarTarget = true;
    public Transform target;
    public float distancia;
    protected bool ataqueSalto = false;
    public bool vivo = true;
    // public void Awake()
    // {

    //     StartCoroutine(CalcularDistancia());
    // }
    protected void Start()
    {
        Debug.Log("Inicio");
        /*
        if (autoseleccionarTarget)
            target = Personaje.singleton.transform;*/
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void LateUpdate()
    { //se ejecuta despues del update
        CheckEstado();
    }
    private void CheckEstado()
    {
        switch (estado)
        {
            case EstadosEF.idle:
                EstadoIdle();
                break;
            case EstadosEF.seguir:
                transform.LookAt(target, Vector3.up);
                EstadoSeguir();
                break;
            case EstadosEF.atacar:
                EstadoAtacar();
                break;
            case EstadosEF.muerto:
                EstadoMuerto();
                break;
            case EstadosEF.jump:
                EstadoAtaqueSalto();
                break;
            default:
                break;
        }

    }
    public void CambiarEstado(EstadosEF e)
    {
        switch (e)
        {
            case EstadosEF.idle:

                break;
            case EstadosEF.seguir:

                break;
            case EstadosEF.atacar:

                break;
            case EstadosEF.muerto:
                vivo = false;
                break;
            case EstadosEF.jump:
                ataqueSalto = true;
                break;
            default:
                break;
        }
        estado = e;
    }
    public virtual void EstadoIdle()
    {
        if (distancia < distanciaSeguir)
        {
            CambiarEstado(EstadosEF.seguir);
        }

    }
    public virtual void EstadoSeguir()
    {
        if (distancia < distanciaAtacar)
        {
            CambiarEstado(EstadosEF.atacar);
        }
        else if (distancia > distanciaEscapar)
        {
            CambiarEstado(EstadosEF.idle);
        }

    }
    public virtual void EstadoAtacar()
    {
        if (distancia > distanciaAtacar + 0.4f)
        {
            CambiarEstado(EstadosEF.seguir);
        }
    }
    public virtual void EstadoAtaqueSalto()
    {

    }
    public virtual void EstadoMuerto()
    {

    }

    protected void Update()
    {
        if (vivo)
        {
            //yield return new WaitForSeconds(0.02f);
            if (target != null)
            {

                distancia = Vector3.Distance(transform.position, target.position);

            }
        }
    }
#if UNITY_EDITOR //estos son cosas que solo van a aparecer en el editor
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanciaAtacar);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position,Vector3.up, distanciaSeguir);
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position,Vector3.up, distanciaEscapar);
    }
#endif
    private void OnDrawGizmos() //todas las texturas que le queramos poner al enemigo se ponen dentro de la carpeta assets/Gizmos
    {
        int icono = (int)estado;
        icono++;
        //Gizmos.DrawIcon(transform.position + Vector3.up * 1.2, 'name' + icono + '.png', false);


    }

}
public enum EstadosEF
{ //estos van a ser los EstadosEF de nuestros enemigos
    idle = 0,
    seguir = 1,
    atacar = 2,
    muerto = 3,
    jump = 4
}