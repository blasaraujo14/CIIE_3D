using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public void GameScene(){

        SceneManager.LoadScene("Escena 1", LoadSceneMode.Single);

    }

    public void OptionScene(){



        SceneManager.LoadScene(6, LoadSceneMode.Single);

    }

    public void QuitGame(){

        Application.Quit();

    }
}
