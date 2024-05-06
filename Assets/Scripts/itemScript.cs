using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class itemScript : MonoBehaviour
{
    GameManager gm;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 10f;
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 )
            gm.destruye(transform.parent.gameObject.GetInstanceID());
    }

    private void OnTriggerEnter(Collider other)
    {
        gm.recoge(this.gameObject.name);
        gm.destruye(transform.parent.gameObject.GetInstanceID());
    }
}
