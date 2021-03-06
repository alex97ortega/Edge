﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activador : MonoBehaviour {

    public Obstaculo obstaculo;
    public bool unaSolaVez;

    bool done = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!unaSolaVez || (unaSolaVez && !done))
        {
            done = true;
            if (other.GetComponent<PlayerController>() != null)
            {
                obstaculo.Activar();
            }

            if(unaSolaVez)
                transform.position -= new Vector3(0, 0.2f, 0);
        }
    }

    public void ResetActivador()
    {
        if(unaSolaVez && done)
        {
            done = false;
            transform.position += new Vector3(0, 0.2f, 0);
        }
    }
}
