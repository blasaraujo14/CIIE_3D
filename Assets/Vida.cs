using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Vida : MonoBehaviour
{
    // Start is called before the first frame update

    public Image vida;

    public Image hit;

    private float currvida;

    public float maxVida;

    public float damage;

    void Start()
    {     
        currvida = maxVida;
        vida.fillAmount = currvida/maxVida;
  
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Q)){//recive daño
            if(currvida > 0){           
                currvida -= damage;
                var color = hit.color;

                color.a = 0.1f;

                hit.color = color;
            
            }
            else{           
                currvida = 0;
                
            }
            vida.fillAmount = currvida/maxVida;

        }

        if(hit.color.a > 0){
                var color = hit.color;

                color.a -= 0.001f;

                hit.color = color;

        }
        
    }

}
