using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : ScriptableObject
{
    //float[,] LIMITES = new float[2, 2] { { 40, -12 }, { -12, 40 } };
    public float[,] LIMITES = new float[2, 2];
    public int enemigosA;
    public int enemigosB;
    public int enemigosC;
    public int enemigosD;
    public bool jefe;
    public Vector3 origen;
    public Vector3 posPortal;
    public string sigEscena;
}
