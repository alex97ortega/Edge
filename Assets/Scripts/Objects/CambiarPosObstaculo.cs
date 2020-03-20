using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarPosObstaculo : MonoBehaviour {


    public Vector3 newPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Obstaculo>()!=null)
        {
            other.GetComponent<Obstaculo>().CambiaPos(newPos);
        }
    }
}
