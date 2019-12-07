using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SueloQuebradizo : MonoBehaviour {

    float timer = 0.5f;
    float cont = 0;
    bool fall = false;
    Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;    
    }
    // Update is called once per frame
    void Update () {
        if (fall)
        {
            cont += Time.deltaTime;
            if (cont >= timer)
            {
                transform.position -= new Vector3(0, 0.5f, 0);
                if (transform.position.y <= -15)
                {
                    fall = false;
                }
            }
        }
	}
    private void OnTriggerEnter(Collider other)
    {
        fall = true;
    }
    public void RestartBlock()
    {
        transform.position = initialPos;
        cont = 0;
    }
}
