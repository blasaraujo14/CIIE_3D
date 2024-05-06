using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    private Vector2 angle = new Vector2(90 * Mathf.Deg2Rad, 0); //angulo en el que la camara esta rotando(se inicializa y se convierte a gradianes)
    private new Camera camera;
    private Vector2 nearPlaneSize; //tamaño del plano cercano

    public Transform follow; //objeto al que va a seguir la camara(debe ser un objeto vacio)
    public float maxDistance; //distancia a la que vamos a tener la camara
    public Vector2 sensitivity; //esto va a ser la sensibilidad de la camara

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //vamos a bloquear el cursor para que no salga de la ventana del juego
        camera = GetComponent<Camera>();

        CalculateNearPlaneSize();
    }

    private void CalculateNearPlaneSize() //calculamos el plano cercano a la camara
    { //fieldOfView es el angulo de la camara y nearClipPlane es la distancia al plano cercano
        float height = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * camera.nearClipPlane;
        float width = height * camera.aspect;

        nearPlaneSize = new Vector2(width, height);
    }

    private Vector3[] GetCameraCollisionPoints(Vector3 direction) //obtenemos puntos de la camara
    { //calculamos como un cuadrado a una distancia de la cabeza del jugador de un nearplane + un poco mas
        Vector3 position = follow.position;
        Vector3 center = position + direction * (camera.nearClipPlane + 0.2f);

        Vector3 right = transform.right * nearPlaneSize.x;
        Vector3 up = transform.up * nearPlaneSize.y;

        return new Vector3[]
        {
            center - right + up,
            center + right + up,
            center - right - up,
            center + right - up
        };
    }

    void Update()
    {
        float hor = Input.GetAxis("Mouse X"); //obtenemos la posicion del mouse en x

        if (hor != 0)
        {
            angle.x += hor * Mathf.Deg2Rad * sensitivity.x; //convertimos a radianes
        }

        float ver = Input.GetAxis("Mouse Y"); //lo mismo para el ángulo y

        if (ver != 0)
        {
            angle.y += ver * Mathf.Deg2Rad * sensitivity.y;
            angle.y = Mathf.Clamp(angle.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad); //hacemos que la camara solo se pueda mover entre dos valores (para que no pase al otro ladod el jugador)
            //limitamos la camara entre dos valores (valor, min, max)
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle.x) * Mathf.Cos(angle.y),
            -Mathf.Sin(angle.y),//el menos es porque tenemos invertida la cámara
            -Mathf.Sin(angle.x) * Mathf.Cos(angle.y)
            );
        RaycastHit hit;
        float distance = maxDistance;
        Vector3[] points = GetCameraCollisionPoints(direction);

        foreach (Vector3 point in points) //vamos a hacer 4 raycast, uno por cada una de las esquinas de la cámara, no se vea parte del interior de los objetos 
        {
            if (Physics.Raycast(point, direction, out hit, maxDistance)) //detectamos si hubo una colisión entre la cabeza del jugador y donde debería estar la cámara
            {
                distance = Mathf.Min((hit.point - follow.position).magnitude, distance); //calculamos la magnitud(distancia entre el jugador y la colisión), y seleccionamos
                //el menor entre la distancia y la distancia de colisión
            }
        }

        transform.position = follow.position + direction * distance; //obtenemos la posicion de la camara
        transform.rotation = Quaternion.LookRotation(follow.position - transform.position); //rotamos la camara para que siga al jugador
    }
}
