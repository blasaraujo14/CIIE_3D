using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PauseMEnu : MonoBehaviour
{
    public void Continuar(){
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        //Instantiate(activator);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().enabled = true;
    }

    public void Opciones(){

        SceneManager.LoadScene("OptionsMain", LoadSceneMode.Additive);
        //SceneManager.UnloadSceneAsync("MenuPausa");
    }


    public void QuitGame(){

        SceneManager.LoadScene("MenuPrincipal");
    }
}
