using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LevelChangeLogic : MonoBehaviour
{
    // Start is called before the first frame update

    public bool LevelChange;

    public int CurrentLevel;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelChange){
            
            if (CurrentLevel > 2){
                CurrentLevel = 0;
            }
            else{
                CurrentLevel = CurrentLevel + 1;
                }
            
            OnLevelChange(CurrentLevel);

        }
        
    }

    public void OnLevelChange(int level){

        SceneManager.LoadScene(level);

    }
}
