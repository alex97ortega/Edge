using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activador : MonoBehaviour {

    public Obstaculo obstaculo;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
            obstaculo.Activar();
    }
}
