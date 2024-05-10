using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new map info", menuName = "map info")]
public class MapInfo : ScriptableObject
{
    //float[,] LIMITES = new float[2, 2] { { 40, -12 }, { -12, 40 } };
    public float LIMITE11;
    public float LIMITE12;
    public float LIMITE21;
    public float LIMITE22;
    public int enemigosA;
    public int enemigosB;
    public int enemigosC;
    public int enemigosD;
    public float ALTURAITEMS;
    public float ALTURAENEMY;
    public bool jefe;
    public Vector3 origen;
    public Vector3 posPortal;
    public string sigEscena;
    public float distSuelo;
}
