using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MenuMuerte : MonoBehaviour
{
    
    private void Start()
    {
        AudioSource source = gameObject.GetComponent<AudioSource>();
        source.volume = PlayerPrefs.GetFloat("SFX");
        source.Play();
    }
    public void Reiniciar(){
        
        //recargar escena principal
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void Salir(){

        Application.Quit();

    }
}
