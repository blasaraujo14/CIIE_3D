using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class camera_script : MonoBehaviour
{
    [SerializeField] Transform pibe;
    [SerializeField] float distance = 5;
    [SerializeField] float sensibilidad = 3;

    float rY;
    float rX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rY += Input.GetAxis("Mouse X");
        rX += Input.GetAxis("Mouse Y");

        Quaternion mouseRotation = Quaternion.Euler(-rX*sensibilidad, rY*sensibilidad, 0);

        transform.position = pibe.position - mouseRotation*Vector3.forward*distance;
        transform.rotation = mouseRotation;
    }
}
