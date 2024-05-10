using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MapInfo map;
    asuna pp;
    float itemTimer;
    float enemyTimer;
    ArrayList activeItems;
    //float[,] LIMITES = new float[2,2] { { 40, -12 }, { -12, 40 } };
    float[,] LIMITES;
    float ALTURAITEMS;
    float ALTURAENEMY;
    GameObject ph;
    itemsCollider nose;
    Vector3 phPosition;
    UnityEngine.Object[] itemPrefabs;
    UnityEngine.Object[] enemyPrefabs;
    public GameObject jefe;
    int numEnemigos;
    int enemigosSpawn;
    Text numEnemigosText;
    CameraOrbit cameraScript;
    AudioSource musica;
    public AudioSource sfx;
    float distSuelo;
    GameObject mapObj;

    // Start is called before the first frame update
    void Start()
    {
        /*******aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa***************/
        //if(PlayerPrefs.GetString("mapInfo")==null) map = (MapInfo)Resources.Load("InfoMapas/" + SceneManager.GetActiveScene().name);
        /*******aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa***************/

        map = (MapInfo)Resources.Load("InfoMapas/" + PlayerPrefs.GetString("mapInfo"));
        PlayerPrefs.SetString("mapInfo", map.sigEscena);
        PlayerPrefs.Save();
        pp = Instantiate(Resources.Load("PrefabJugador"), map.origen, Quaternion.identity).GetComponent<asuna>();
        if (map.jefe) jefe = (GameObject)Instantiate(Resources.Load("finalboss"), new Vector3(0,1,0), Quaternion.identity);
        //pp = GameObject.FindGameObjectWithTag("Player").GetComponent<asuna>();
        itemTimer = 5;
        enemyTimer = 5;
        activeItems = new ArrayList();
        LIMITES = new float[2, 2] { { map.LIMITE11, map.LIMITE12 }, { map.LIMITE21, map.LIMITE22 } };
        ALTURAITEMS = map.ALTURAITEMS;
        ALTURAENEMY = map.ALTURAENEMY;
        //ph = (GameObject)Instantiate(Resources.Load("placeholder"), new Vector3(0,-12,0), Quaternion.identity, transform);
        ph = GameObject.Find("placeholder");
        nose = ph.GetComponent<itemsCollider>();
        itemPrefabs = Resources.LoadAll("Items");
        enemyPrefabs = Resources.LoadAll("Enemigos");
        numEnemigos = enemigosSpawn = map.enemigosA+map.enemigosB+map.enemigosC+map.enemigosD;
        numEnemigosText = ((GameObject)Instantiate(Resources.Load("UI"))).transform.Find("Enemigos").Find("Text (Legacy)").gameObject.GetComponent<Text>();
        numEnemigosText.text = numEnemigos.ToString();
        phPosition = ph.transform.position;
        distSuelo = map.distSuelo;
        GameObject mainCamera = (GameObject)Instantiate(Resources.Load("Camera"));
        cameraScript = mainCamera.GetComponent<CameraOrbit>();
        musica = mainCamera.GetComponent<AudioSource>();
        sfx = mainCamera.transform.Find("SFX").gameObject.GetComponent<AudioSource>();
        ChangeVolume();
        if (map.jefe)
        {
            cameraScript.enabled = false;
            pp.enabled = false;
            Instantiate(Resources.Load("AnimacionJefeFinal"));
        }
        //portal = GameObject.Find("Dark Singularity");
    }

    // Update is called once per frame
    void Update()
    {
        if (pp.transform.position.y < distSuelo)
        {
            pp.transform.position = new Vector3(pp.transform.position.x, distSuelo + 1f, pp.transform.position.z);
        }
        if (itemTimer < 0 && numEnemigos > 0)
        {
            itemTimer = 5;
            activeItems.Add(spawn((GameObject)itemPrefabs[UnityEngine.Random.Range(0, itemPrefabs.Length)], ALTURAITEMS));
        }
        else itemTimer -= Time.deltaTime;
        if (enemigosSpawn > 0 && enemyTimer < 0)
        {
            enemigosSpawn--;
            enemyTimer = 5;
            activeItems.Add(spawn((GameObject)enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], ALTURAENEMY));
        }
        else enemyTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuPausa", LoadSceneMode.Additive);
            enabled = false;
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        if (pp != null) pp.enabled = false;
        if (cameraScript != null) cameraScript.enabled = false;
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        if (pp != null) pp.enabled = true;
        if(cameraScript != null) cameraScript.enabled = true;
    }

    GameObject spawn(GameObject prefab, float altura)
    {
        do
        {
            ph.transform.position = new Vector3(
                UnityEngine.Random.Range(LIMITES[0, 0], LIMITES[1, 0]), altura, UnityEngine.Random.Range(LIMITES[0, 1], LIMITES[1, 1]));
        } while (nose.nose);
        Vector3 posItem = ph.transform.position;
        GameObject nuevo = GameObject.Instantiate(prefab, posItem, Quaternion.identity);
        ph.transform.position = phPosition;
        //return nuevo.GetInstanceID();
        return nuevo;
    }

    public void recoge(string name)
    {
        pp.cambiaArma(name);
    }

    public void destruye(GameObject obj)
    {
        /*
        int[] a = new int[] { id };
        GameObject.SetGameObjectsActive(new Unity.Collections.NativeArray<int>(a, Allocator.Temp), false);
        Debug.Log("destruido");
        */
        //GameObject obj = (GameObject)EditorUtility.InstanceIDToObject(id);
        activeItems.Remove(obj);
        if (obj.tag == "Enemigo")
        {
            if (--numEnemigos == 0)
            {
                Instantiate(Resources.Load("Dark Singularity"), map.posPortal, Quaternion.identity);
            }
            numEnemigosText.text = numEnemigos.ToString();
        }
        GameObject.Destroy(obj);
    }
    public void Muerte()
    {
        SceneManager.LoadScene("MenuMuerte", LoadSceneMode.Additive);
        enabled = false;
    }

    public void ChangeVolume()
    {

        musica.volume = PlayerPrefs.GetFloat("Musica");
        sfx.volume = PlayerPrefs.GetFloat("SFX");
    }
}
