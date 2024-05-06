using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    // Start is called before the first frame update
    LayerMask mask;

    public float distance = 20f;


    public GameObject  TextoCambioNivel;

    GameObject ultimoReconocido = null;

    void Start()
    {
        mask = LayerMask.GetMask("RayCast");
        TextoCambioNivel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,distance,mask)){
            Deselect();
            
            if(hit.collider.tag == "LevelChange" ){
                
                SelectedObject(hit.transform);
                if(Input.GetKeyDown(KeyCode.F)){

                    hit.collider.transform.GetComponent<ObjectSel>().ObjectClicked();
                    Deselect();
                }

            }
          

        }  
        else{

                Deselect();

            }
        
    }

    void SelectedObject(Transform transform){

        ultimoReconocido = transform.gameObject;

    }

    void Deselect(){

        if(ultimoReconocido){

            ultimoReconocido = null;

        }

    }

    void OnGUI(){

        if(ultimoReconocido){

            TextoCambioNivel.SetActive(true);

        }
        else{

            TextoCambioNivel.SetActive(false);

        }
    }
}
