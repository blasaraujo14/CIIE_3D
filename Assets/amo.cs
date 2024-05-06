using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class amo : MonoBehaviour
{
    // Start is called before the first frame update

    public int maxAmo;//municion maxima

    private int currAmo;//municion en el cargador

    private int totalAmo;//municion restante menos el cargador

    public int maxCharger;//municion maxima del cargador

    public Text currAmoText;

    public Text totalAmoText;

    void Start()
    {
        currAmo = maxCharger;
        totalAmo = maxAmo - currAmo;
        
        totalAmoText.text = totalAmo.ToString();
        currAmoText.text = currAmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.R)){//recarga

            totalAmo = totalAmo - maxCharger;
            currAmo = maxCharger;
            totalAmoText.text = totalAmo.ToString();
            currAmoText.text = currAmo.ToString();

        }

        if(Input.GetKeyDown(KeyCode.T)){//dispara

            if(currAmo > 0){

                currAmo --;
                currAmoText.text = currAmo.ToString();
            }
            else{

                currAmo = 0;
                currAmoText.text = currAmo.ToString();
            }



        }

    }
}
