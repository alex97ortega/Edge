using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pared : MonoBehaviour {

    // método para paredes que pueda "atravesar" el player 
    // si está siendo desplazado por una plataforma
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            other.gameObject.transform.parent = null;
            other.gameObject.GetComponent<PlayerController>().Ajusta();
        }      
    }
}
