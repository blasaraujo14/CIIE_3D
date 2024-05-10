using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource musica;
    AudioSource sfx;
    private void Start()
    {
        musica = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        sfx = GameObject.FindGameObjectWithTag("MainCamera").transform.Find("SFX").gameObject.GetComponent<AudioSource>();
        ChangeVolume();
        PlayerPrefs.SetString("mapInfo", "Nivel1");
        PlayerPrefs.Save();
    }
    public void GameScene(){
        SceneManager.LoadScene("Nivel1", LoadSceneMode.Single);
    }

    public void ChangeVolume()
    {
        musica.volume = PlayerPrefs.GetFloat("Musica");
        sfx.volume = PlayerPrefs.GetFloat("SFX");
    }

    public void OptionScene(){
        SceneManager.LoadScene("OptionsMain", LoadSceneMode.Additive);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
