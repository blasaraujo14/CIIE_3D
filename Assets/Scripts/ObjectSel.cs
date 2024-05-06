using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.SceneManagement;

public class ObjectSel : MonoBehaviour
{
    // Start is called before the first frame update



    public void ObjectClicked(){

        //cambiar para que salte a la siguiente escena

            
            if (SceneManager.GetActiveScene().buildIndex + 1 > 3){
                LevelChange(0);
            }
            else{
                LevelChange(SceneManager.GetActiveScene().buildIndex + 1);
                }
            

    }

    
    public void LevelChange(int level){

        SceneManager.LoadScene(level, LoadSceneMode.Single);

    }

}
