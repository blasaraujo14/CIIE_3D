using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MenuMuerte : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame

    public void Reiniciar(){
        
        //recargar escena principal
        SceneManager.UnloadSceneAsync("MenuMuerte");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Salir(){

        Application.Quit();

    }
}
