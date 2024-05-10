//Firma: @Sander, 2022.

using System.Collections.Generic;
using UnityEngine;
//using StarterAssets;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LineRenderer))]
public class Sensor : MonoBehaviour
{

    [Header("Configuración del Sensor")]
    [Tooltip("Define el tamaño inicial del sensor. Si lo desea, en el resto de las configuraciones podrá dejarlo fijo o que cambie dinamicamente de tamaño.")]
    public float tamañoSensor = 1;
    [Tooltip("El material que se le aplicará al sensor.")]
    public Material material;
    [Tooltip("El sonidoFX que se le aplicará al sensor.")]
    public AudioClip sonidofx;
    [Tooltip("Define el grosor de linea con el que se pintará.")]
    public float lineaGrosor = 0.1f;
    [Tooltip("Define la velocidad de variación del tamaño del sensor.")]
    public float velocidadEscalado = 0.5f;
    [Tooltip("Define la frecuencia de sombreado que tendrá el sensor. Es la unidad de interlineado que se usará para dibujar.")]
    public float frecuenciaSombreado = 1f;
    [Tooltip("Tiempo de vida que estará activo el sensor. (Aplica para las formas de propagación: Bucle y Fijo)")]
    public float tiempo = 2;
    [Tooltip("Define el Tipo de Dibujo del Sensor. Puede ser de tipo Caja Sombreada o Plano Sombreado.")]
    public TipoDibujo tipoDibujo;

    [Space(5)]

    [Header("Forma de Propagación")]
    [Tooltip("Si está activado significa que el sensor se propagará expandiondose solamente como una ráfaga. Su tiempo de vida será mientras no alcance el tamaño limite del sensor. ")]
    public bool rafaga;
    [Tooltip("Si está activado significa que el tamaño del sensor cambiará de forma dinámica de forma tal que se expande y se contrae continuamente como un bucle. Su tiempo de vida estará marcado por la variable tiempo.")]
    public bool bucle;
    [Tooltip("Si está activado el tamaño del sensor siempre será fijo. Su tiempo de vida estará marcado por la variable tiempo.")]
    public bool fijo;

    [Space(5)]

    [Header("Configuración de Ráfaga")]
    [Tooltip("Tamaño máximo limite del sensor. Define hasta donde puede crecer el sensor.")]
    public float tamañoSensorLimite = 40f;

    [Space(5)]

    [Header("Configuración de Bucle")]
    [Tooltip("Define el valor con el que variará el tamaño del sensor.")]
    public float valorBarrido = 10;


    private LineRenderer _lineRenderer;
    private BoxCollider _boxCollider;
    private AudioSource _audioSource;
    private float _barridoSuperior;
    private float _barridoInferior;
    private float _scale;
    private bool _estoyExpandiendo = true;
    private float _diferenciaDeAjuste = 0f;

    // Variables para opciones extras
    private float _tamañoSensorRestaurar;


    private void Start()
    {
        _tamañoSensorRestaurar = tamañoSensor;
    }

    private void OnEnable()
    {
        Debug.Log("enableado");
        _lineRenderer = GetComponent<LineRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = sonidofx;
        _boxCollider.isTrigger = true;
        _lineRenderer.startWidth = lineaGrosor;
        _lineRenderer.material = material;
        _lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        this._barridoSuperior = tamañoSensor + valorBarrido;
        this._barridoInferior = tamañoSensor - valorBarrido;
        this._scale = tamañoSensor;

        if (bucle)
        {
            Debug.Log("entró");
            Invoke("DesactivarSensor", tiempo);
            _audioSource.loop = true;
        }

        else if (fijo)
        {
            Invoke("DesactivarSensor", tiempo);
            _audioSource.loop = false;
        }

        else if (rafaga)
        {
            _audioSource.loop = false;
        }
        _audioSource.Play();
    }

    private void FixedUpdate()
    {
        this.transform.rotation = Quaternion.identity;
        _boxCollider.size = tipoDibujo.Equals(TipoDibujo.Caja_Sombreada) ? new Vector3(tamañoSensor, tamañoSensor, tamañoSensor) : new Vector3(tamañoSensor, 0.5f, tamañoSensor); // Validacion del collider fisico en dependencia de si es de tipo caja o plano
        _boxCollider.center = tipoDibujo.Equals(TipoDibujo.Caja_Sombreada) ? new Vector3(0, (tamañoSensor / 2), 0) : new Vector3(0, 0.25f, 0); // Validacion del collider fisico en dependencia de si es de tipo caja o plano

        List<Vector3> posiciones = new List<Vector3>();

        if (tipoDibujo.Equals(TipoDibujo.Caja_Sombreada))  // Si lo que quiero dibujar es la caja sombreada
        {
            posiciones = DibujarCajaSombreada(this.transform.position);
        }

        else if (tipoDibujo.Equals(TipoDibujo.Plano_Sombreado)) // Si lo que quiero dibujar es el plano sombreado
        {
            posiciones = DibujarPlanoSombreado(this.transform.position, 'x', 'z', true, true);
        }

        _lineRenderer.positionCount = posiciones.Count;
        _lineRenderer.SetPositions(posiciones.ToArray());


        if (bucle)
        {
            if (Mathf.Abs(_scale - _barridoSuperior) < 1 || Mathf.Abs(_scale - _barridoInferior) < 1)
            {
                _estoyExpandiendo = !_estoyExpandiendo;

            }

            if (_scale != _barridoSuperior && _estoyExpandiendo)
            {
                _scale = Mathf.Lerp(_scale, _barridoSuperior, velocidadEscalado * Time.deltaTime);

            }
            else if (_scale != _barridoInferior && !_estoyExpandiendo)
            {
                _scale = Mathf.Lerp(_scale, _barridoInferior, velocidadEscalado * Time.deltaTime);

            }


            tamañoSensor = _scale;
        }

        else if (rafaga)
        {
            if (Mathf.Abs(_scale - tamañoSensorLimite) < 1)
            {
                DesactivarSensor();
            }

            else if (tamañoSensor <= tamañoSensorLimite)
            {
                _scale = Mathf.Lerp(_scale, tamañoSensorLimite, velocidadEscalado * Time.deltaTime);
            }

            tamañoSensor = _scale;
        }


    }

    private void DesactivarSensor()
    {
        tamañoSensor = _tamañoSensorRestaurar;
        _scale = tamañoSensor;
        this.GetComponentInParent<finalboss>().sensor = false;
        this.gameObject.SetActive(false);

    }

    private List<Vector3> DibujarCajaSombreada(Vector3 posicion)  // Se dibuja siempre teniendo como punto centro al personaje
    {
        // Aquí simplemente vamos llamando al método que dibuja un plano sombreado ... pero lo hacemos para cada par de planos .. y así vamos construyendo la caja sombreada

        List<Vector3> posiciones = new List<Vector3>();
        List<Vector3> planoXZ1 = DibujarPlanoSombreado(posicion, 'x', 'z', true, true);
        List<Vector3> planoXZ2 = DibujarPlanoSombreado(posicion, 'x', 'z', true, false);
        List<Vector3> planoZY1 = DibujarPlanoSombreado(posicion, 'z', 'y', true, true);
        List<Vector3> planoZY2 = DibujarPlanoSombreado(posicion, 'z', 'y', false, true);
        List<Vector3> planoXY1 = DibujarPlanoSombreado(posicion, 'x', 'y', true, true);
        List<Vector3> planoXY2 = DibujarPlanoSombreado(posicion, 'x', 'y', false, true);

        posiciones.AddRange(planoXZ1);
        posiciones.Add(planoXZ1[0]);

        posiciones.AddRange(planoZY1);
        posiciones.Add(planoZY1[3]);

        posiciones.AddRange(planoXY1);
        posiciones.Add(planoXY1[3]);

        posiciones.AddRange(planoZY2);
        posiciones.Add(planoZY2[3]);

        posiciones.AddRange(planoXY2);
        posiciones.Add(planoXY2[2]);
        posiciones.Add(planoZY1[2]);

        posiciones.AddRange(planoXZ2);


        return posiciones;
    }

    private List<Vector3> DibujarPlanoSombreado(Vector3 posicion, char eje1, char eje2, bool eje1PositivoDerecha, bool eje2PositivoArriba) // Se dibuja siempre teniendo como punto centro al personaje
    {
        List<Vector3> puntos = new List<Vector3>();
        Vector3 punto1 = new Vector3();
        Vector3 punto2 = new Vector3();
        Vector3 punto3 = new Vector3();
        Vector3 punto4 = new Vector3();

        // Primero dibujamos el plano  (Siempre tomamos como referencia inicial la esquina inferior izquierda)
        if (eje1 == 'x' && eje2 == 'z')
        {
            if (eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y, posicion.z - (tamañoSensor / 2));
                punto2 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y, posicion.z + (tamañoSensor / 2));
                punto3 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y, posicion.z + (tamañoSensor / 2));
                punto4 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y, posicion.z - (tamañoSensor / 2));
            }
            else if (eje1PositivoDerecha && !eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z + (tamañoSensor / 2));
                punto2 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z - (tamañoSensor / 2));
                punto3 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z - (tamañoSensor / 2));
                punto4 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z + (tamañoSensor / 2));
            }

        }
        else if (eje1 == 'z' && eje2 == 'y')
        {
            if (eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y, posicion.z - (tamañoSensor / 2));
                punto2 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z - (tamañoSensor / 2));
                punto3 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z + (tamañoSensor / 2));
                punto4 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y, posicion.z + (tamañoSensor / 2));
            }
            else if (!eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y, posicion.z + (tamañoSensor / 2));
                punto2 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z + (tamañoSensor / 2));
                punto3 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z - (tamañoSensor / 2));
                punto4 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y, posicion.z - (tamañoSensor / 2));
            }

        }
        else if (eje1 == 'x' && eje2 == 'y')
        {
            if (eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y, posicion.z + (tamañoSensor / 2));
                punto2 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z + (tamañoSensor / 2));
                punto3 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z + (tamañoSensor / 2));
                punto4 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y, posicion.z + (tamañoSensor / 2));
            }
            else if (!eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y, posicion.z - (tamañoSensor / 2));
                punto2 = new Vector3(posicion.x + (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z - (tamañoSensor / 2));
                punto3 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y + tamañoSensor, posicion.z - (tamañoSensor / 2));
                punto4 = new Vector3(posicion.x - (tamañoSensor / 2), posicion.y, posicion.z - (tamañoSensor / 2));
            }

        }


        puntos.Add(punto1);
        puntos.Add(punto2);
        puntos.Add(punto3);
        puntos.Add(punto4);
        puntos.Add(punto1);


        // Ahora empieza el ciclo moviendome en un eje (eje1) una unidad, luego hacia el otro eje (eje2) en la misma unidad, luego me muevo en ese eje (eje2) una unidad mas y luego hacia el otro eje (eje1) en esa nueva unidad y asi sucesivamente...
        float contadorUnidad = frecuenciaSombreado;
        bool esEje1 = true; // Inicialmente me muevo en eje1
        bool huboDiferenciaDeAjuste = false;   // Esta variable va a controlar en que momento hubo que ajustar la frecuencia de sombreado para pintar la ultima diagonal (Validación para los casos decimales, asegurarnos que no se pinte fuera de los limites del cuadrado)

        while (contadorUnidad <= tamañoSensor)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 puntoActual = new Vector3();

                if (eje1 == 'x' && eje2 == 'z')
                {
                    if (eje1PositivoDerecha && eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(punto1.x + contadorUnidad, punto1.y, punto1.z); // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)

                        }

                        else
                        {
                            puntoActual = new Vector3(punto1.x, punto1.y, punto1.z + contadorUnidad);  // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)
                        }
                    }

                    else if (eje1PositivoDerecha && !eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(punto1.x + contadorUnidad, punto1.y, punto1.z); // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)

                        }

                        else
                        {
                            puntoActual = new Vector3(punto1.x, punto1.y, punto1.z - contadorUnidad);  // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)
                        }
                    }
                }

                else if (eje1 == 'z' && eje2 == 'y')
                {
                    if (eje1PositivoDerecha && eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(punto1.x, punto1.y, punto1.z + contadorUnidad); // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)

                        }

                        else
                        {
                            puntoActual = new Vector3(punto1.x, punto1.y + contadorUnidad, punto1.z);  // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)
                        }
                    }

                    else if (!eje1PositivoDerecha && eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(punto1.x, punto1.y, punto1.z - contadorUnidad); // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)

                        }

                        else
                        {
                            puntoActual = new Vector3(punto1.x, punto1.y + contadorUnidad, punto1.z);  // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)
                        }
                    }
                }

                else if (eje1 == 'x' && eje2 == 'y')
                {
                    if (eje1PositivoDerecha && eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(punto1.x + contadorUnidad, punto1.y, punto1.z); // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)

                        }

                        else
                        {
                            puntoActual = new Vector3(punto1.x, punto1.y + contadorUnidad, punto1.z);  // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)
                        }
                    }

                    else if (!eje1PositivoDerecha && eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(punto1.x - contadorUnidad, punto1.y, punto1.z); // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)

                        }

                        else
                        {
                            puntoActual = new Vector3(punto1.x, punto1.y + contadorUnidad, punto1.z);  // Punto 1 como referencia inicial (Siempre tomamos la esquina inferior izquierda como primer punto para cada plano)
                        }
                    }
                }


                if (i == 0)
                    esEje1 = !esEje1;

                puntos.Add(puntoActual);
            }

            if ((contadorUnidad + frecuenciaSombreado > tamañoSensor) && (!huboDiferenciaDeAjuste) && (contadorUnidad != tamañoSensor))  // Si nos pasamos del limite del cuadrado y no hemos dibujado aún la diagonal principal
            {
                _diferenciaDeAjuste = tamañoSensor - contadorUnidad;
                contadorUnidad += _diferenciaDeAjuste;
                huboDiferenciaDeAjuste = true;
            }
            else                                                                              // Si estamos dentro de los limites del cuadrado
            {
                contadorUnidad += frecuenciaSombreado;
            }


        }

        return DibujarPlanoSombreadoSegundaMitad(puntos, eje1, eje2, eje1PositivoDerecha, eje2PositivoArriba);
    }

    private List<Vector3> DibujarPlanoSombreadoSegundaMitad(List<Vector3> puntosPrimeraMitad, char eje1, char eje2, bool eje1PositivoDerecha, bool eje2PositivoArriba)
    {
        List<Vector3> puntos = puntosPrimeraMitad;
        Vector3 segundoVerticeDiagonal = puntos[2];
        puntos.Add(segundoVerticeDiagonal);

        // Ahora empieza el ciclo moviendome en un eje (eje1) una unidad, luego hacia el otro eje (eje2) en la misma unidad, luego me muevo en ese eje (eje2) una unidad mas y luego hacia el otro eje (eje1) en esa nueva unidad y asi sucesivamente...
        float contadorUnidad = frecuenciaSombreado;
        bool esEje1 = true; // Inicialmente me muevo en eje1
        bool huboDiferenciaDeAjuste = false;  // Esta variable va a controlar en que momento hubo que ajustar la frecuencia de sombreado para pintar la ultima diagonal (Validación para los casos decimales, asegurarnos que no se pinte fuera de los limites del cuadrado)

        while (contadorUnidad <= tamañoSensor)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 puntoActual = new Vector3();

                if (eje1 == 'x' && eje2 == 'z')
                {
                    if (eje1PositivoDerecha && eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x - contadorUnidad, segundoVerticeDiagonal.y, segundoVerticeDiagonal.z); // Punto 3 como vértice del otro extremo del cuadrado

                        }

                        else
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x, segundoVerticeDiagonal.y, segundoVerticeDiagonal.z - contadorUnidad);  // Punto 3 como vértice del otro extremo del cuadrado
                        }
                    }

                    else if (eje1PositivoDerecha && !eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x - contadorUnidad, segundoVerticeDiagonal.y, segundoVerticeDiagonal.z); // Punto 3 como vértice del otro extremo del cuadrado

                        }

                        else
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x, segundoVerticeDiagonal.y, segundoVerticeDiagonal.z + contadorUnidad);  // Punto 3 como vértice del otro extremo del cuadrado
                        }
                    }
                }

                else if (eje1 == 'z' && eje2 == 'y')
                {
                    if (eje1PositivoDerecha && eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x, segundoVerticeDiagonal.y, segundoVerticeDiagonal.z - contadorUnidad); // Punto 3 como vértice del otro extremo del cuadrado

                        }

                        else
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x, segundoVerticeDiagonal.y - contadorUnidad, segundoVerticeDiagonal.z);  // Punto 3 como vértice del otro extremo del cuadrado
                        }
                    }

                    else if (!eje1PositivoDerecha && eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x, segundoVerticeDiagonal.y, segundoVerticeDiagonal.z + contadorUnidad); // Punto 3 como vértice del otro extremo del cuadrado

                        }

                        else
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x, segundoVerticeDiagonal.y - contadorUnidad, segundoVerticeDiagonal.z);  // Punto 3 como vértice del otro extremo del cuadrado
                        }
                    }
                }

                else if (eje1 == 'x' && eje2 == 'y')
                {
                    if (eje1PositivoDerecha && eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x - contadorUnidad, segundoVerticeDiagonal.y, segundoVerticeDiagonal.z); // Punto 3 como vértice del otro extremo del cuadrado

                        }

                        else
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x, segundoVerticeDiagonal.y - contadorUnidad, segundoVerticeDiagonal.z);  // Punto 3 como vértice del otro extremo del cuadrado
                        }
                    }

                    else if (!eje1PositivoDerecha && eje2PositivoArriba)
                    {
                        if (esEje1)
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x + contadorUnidad, segundoVerticeDiagonal.y, segundoVerticeDiagonal.z); // Punto 3 como vértice del otro extremo del cuadrado

                        }

                        else
                        {
                            puntoActual = new Vector3(segundoVerticeDiagonal.x, segundoVerticeDiagonal.y - contadorUnidad, segundoVerticeDiagonal.z);  // Punto 3 como vértice del otro extremo del cuadrado
                        }
                    }
                }

                if (i == 0)
                    esEje1 = !esEje1;

                puntos.Add(puntoActual);
            }

            if ((contadorUnidad + frecuenciaSombreado > tamañoSensor) && (!huboDiferenciaDeAjuste) && (contadorUnidad != tamañoSensor))  // Si nos pasamos del limite del cuadrado y no hemos dibujado aún la diagonal principal
            {
                _diferenciaDeAjuste = tamañoSensor - contadorUnidad;
                contadorUnidad += _diferenciaDeAjuste;
                huboDiferenciaDeAjuste = true;
            }
            else                                                      // Si estamos dentro de los limites del cuadrado
            {
                contadorUnidad += frecuenciaSombreado;
            }

        }

        return puntos;
    }
}

public enum TipoDibujo
{
    Caja_Sombreada,
    Plano_Sombreado
}