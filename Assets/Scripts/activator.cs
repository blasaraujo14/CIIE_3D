using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().enabled = true;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
