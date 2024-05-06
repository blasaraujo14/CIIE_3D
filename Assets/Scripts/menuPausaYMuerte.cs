using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class moverse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


        
    }

    // Update is called once per frame
    void FixedUpdate() // con el Update normal no funciona
    {
           //float velocidad = 0.05f;

           /* if(transform.position.z <= -12f){
                transform.position = transform.position + new Vector3(0, 0, 1f); 
                velocidad = 0.05f;
            }
            if(transform.position.z >= 45f){
                transform.position = transform.position + new Vector3(0, 0, -1f); 
                velocidad = -0.05f;
            } 
            
            transform.position = transform.position + new Vector3(0, 0, velocidad); */

        if(Input.GetKeyDown(KeyCode.Escape)){

            
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(5, LoadSceneMode.Additive);

        }

        if(Input.GetKeyDown(KeyCode.M)){

            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(6, LoadSceneMode.Additive);

        }

        
    }
}
