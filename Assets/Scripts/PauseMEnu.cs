using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PauseMEnu : MonoBehaviour
{

    public void Continuar(){

        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("MenuPausa");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Opciones(){

        SceneManager.LoadScene(4, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("MenuPausa");
    }


    public void QuitGame(){

        Application.Quit();

    }
}
