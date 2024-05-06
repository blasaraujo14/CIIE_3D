using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.Audio;

using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioMixer audioMixer;

    public Dropdown dropdownRes;

    Resolution[] resolutions;

    void Start(){

       resolutions = Screen.resolutions;

       dropdownRes.ClearOptions();

       List<string> options = new List<string>();

       int currentResolution =  0;

       for (int i = 0; i <   resolutions.Length; i++){

            string option = resolutions[i].width + "x" + resolutions[i].height;

            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height  ){

                currentResolution = i;

            }

       }

       dropdownRes.AddOptions(options);
       dropdownRes.value = currentResolution;
       dropdownRes.RefreshShownValue();

    }

    public void SetVolume(float volume){

        audioMixer.SetFloat("Volume", volume);


    }

    public void FullScreen(bool isFullScreen){

        Screen.fullScreen = isFullScreen;

    }

    public void SetResolution(int resolution ){
        
        Resolution res = resolutions[resolution];

        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

    }

    public void Volver(){



        SceneManager.UnloadSceneAsync("Opciones");
        SceneManager.LoadScene(5, LoadSceneMode.Additive);


    }

        public void VolverMain(){



        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0, LoadSceneMode.Additive);


    }

}

