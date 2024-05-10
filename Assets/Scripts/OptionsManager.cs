using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.Audio;

using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    // Start is called before the first frame update
    Dropdown dropdownRes;
    Slider sliderMusica;
    Slider sliderSFX;
    Resolution[] resolutions;
    GameObject manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");
        dropdownRes = GameObject.Find("ResolucionSel").GetComponent<Dropdown>();
        sliderMusica = GameObject.Find("VolumeMusic").GetComponent<Slider>();
        sliderSFX = GameObject.Find("VolumeSFX").GetComponent<Slider>();
        resolutions = Screen.resolutions;
        dropdownRes.ClearOptions();

        sliderMusica.value = PlayerPrefs.GetFloat("Musica");
        sliderSFX.value = PlayerPrefs.GetFloat("SFX");

        List<string> options = new List<string>();

        int currentResolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }
        dropdownRes.AddOptions(options);
        dropdownRes.value = currentResolution;
        dropdownRes.RefreshShownValue();
    }

    public void FullScreen(bool isFullScreen)
    {

        Screen.fullScreen = isFullScreen;

    }

    public void SetResolution(int resolution)
    {

        Resolution res = resolutions[resolution];

        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

    }

    public void SetMusic()
    {
        PlayerPrefs.SetFloat("Musica", sliderMusica.value);
    }

    public void SetSFX()
    {
        PlayerPrefs.SetFloat("SFX", sliderSFX.value);
    }
    public void VolverMain(){
        PlayerPrefs.Save();
        if (SceneManager.sceneCount == 2) manager.GetComponent<MainMenu>().ChangeVolume();
        else manager.GetComponent<GameManager>().ChangeVolume();
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount-1));
    }

}

