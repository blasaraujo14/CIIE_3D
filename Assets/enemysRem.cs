using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class enemysRem : MonoBehaviour
{
    // Start is called before the first frame update

    public Text rem;

    public int maxEnem;

    private int currEnem;
    void Start()
    {
        currEnem = maxEnem;
    }

    public void enemyKill(){

        currEnem--;
    }

    // Update is called once per frame
    void Update()
    {
        rem.text = currEnem.ToString();
        if(Input.GetKeyDown(KeyCode.Y)){//muere un enemigo
            if(currEnem > 0){ 

                currEnem--;;
            
            }
            else{  

                currEnem = 0;
                
            }

        }
    }
}
