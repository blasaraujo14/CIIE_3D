using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    asuna pp;
    public float time;
    ArrayList activeItems;
    float[,] LIMITES = new float[2,2] { { 40, -12 }, { -12, 40 } };
    float ALTURAITEMS = 0.5f;
    GameObject ph;
    Vector3 phPosition = new Vector3 (0,-12f,0);
    UnityEngine.Object[] ogPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        pp = GameObject.FindGameObjectWithTag("Player").GetComponent<asuna>();
        time = 5;
        activeItems = new ArrayList();
        ph = GameObject.Find("placeholder");
        ogPrefabs = Resources.LoadAll("Items");
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 0)
        {
            time = 5;
            activeItems.Add(spawnItem());
        }
        else
        {
            time -= Time.deltaTime;
        }
    }

    int spawnItem()
    {
        itemsCollider nose = ph.GetComponent<itemsCollider>();
        do
        {
            ph.transform.position = new Vector3(
                UnityEngine.Random.Range(LIMITES[0, 0], LIMITES[1, 0]), ALTURAITEMS, UnityEngine.Random.Range(LIMITES[0, 1], LIMITES[1, 1]));
        } while (nose.nose);
        Debug.Log(ph.transform.position);
        Vector3 posItem = ph.transform.position;
        GameObject nuevo = GameObject.Instantiate((GameObject)ogPrefabs[UnityEngine.Random.Range(0,ogPrefabs.Length)], posItem, Quaternion.identity);
        ph.transform.position = phPosition;
        return nuevo.GetInstanceID();
    }
    public void recoge(string name)
    {
        pp.cambiaArma(name);
    }

    public void destruye(int id)
    {
        /*
        int[] a = new int[] { id };
        GameObject.SetGameObjectsActive(new Unity.Collections.NativeArray<int>(a, Allocator.Temp), false);
        Debug.Log("destruido");
        */
        activeItems.Remove(id);
        GameObject.Destroy(EditorUtility.InstanceIDToObject(id));
    }
}
