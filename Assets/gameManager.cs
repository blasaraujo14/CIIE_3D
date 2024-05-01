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
    float itemTimer;
    float enemyTimer;
    ArrayList activeItems;
    float[,] LIMITES = new float[2,2] { { 40, -12 }, { -12, 40 } };
    float ALTURAITEMS = 0.5f;
    float ALTURAENEMY = 1f;
    GameObject ph;
    itemsCollider nose;
    Vector3 phPosition = new Vector3 (0,-12f,0);
    UnityEngine.Object[] itemPrefabs;
    UnityEngine.Object[] enemyPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        pp = GameObject.FindGameObjectWithTag("Player").GetComponent<asuna>();
        itemTimer = 5;
        enemyTimer = 5;
        activeItems = new ArrayList();
        ph = GameObject.Find("placeholder");
        nose = ph.GetComponent<itemsCollider>();
        itemPrefabs = Resources.LoadAll("Items");
        enemyPrefabs = Resources.LoadAll("Enemigos");
    }

    // Update is called once per frame
    void Update()
    {
        if (itemTimer < 0)
        {
            itemTimer = 5;
            activeItems.Add(spawn((GameObject)itemPrefabs[UnityEngine.Random.Range(0, itemPrefabs.Length)], ALTURAITEMS));
        }
        else itemTimer -= Time.deltaTime;
        if (enemyTimer < 0)
        {
            enemyTimer = 5;
            activeItems.Add(spawn((GameObject)enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], ALTURAENEMY));
        }
        else enemyTimer -= Time.deltaTime;
    }

    int spawn(GameObject prefab, float altura)
    {
        do
        {
            ph.transform.position = new Vector3(
                UnityEngine.Random.Range(LIMITES[0, 0], LIMITES[1, 0]), altura, UnityEngine.Random.Range(LIMITES[0, 1], LIMITES[1, 1]));
        } while (nose.nose);
        Vector3 posItem = ph.transform.position;
        GameObject nuevo = GameObject.Instantiate(prefab, posItem, Quaternion.identity);
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
