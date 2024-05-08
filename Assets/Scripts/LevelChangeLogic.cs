using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LevelChangeLogic : MonoBehaviour
{
    // Start is called before the first frame update

    public bool LevelChange;

    public int CurrentLevel;
    GameObject texto;

    void Start()
    {
        texto = transform.Find("TextoPortal").gameObject;
        texto.SetActive(false);
        LevelChange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelChange && Input.GetKey(KeyCode.F)){
            
            if (CurrentLevel > 2){
                CurrentLevel = 0;
            }
            else{
                CurrentLevel = CurrentLevel + 1;
                }
            

            OnLevelChange(CurrentLevel);

        }
        
    }

    public void OnLevelChange(int level){

        Debug.Log(SceneManager.loadedSceneCount);
        SceneManager.LoadScene(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().map.sigEscena);

    }
    private void OnTriggerEnter(Collider other)
    {
        texto.SetActive(true);
        LevelChange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        texto.SetActive(false);
        LevelChange = false;
    }
}
