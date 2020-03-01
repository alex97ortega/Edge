using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// cambia la trayectoria de un obstaculo cuando haya concluido su anterior
public class CambiarTrayectoria : MonoBehaviour {
    
    public int newX, newY, newZ;
    public float newDistance;
    public Obstaculo obstaculo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            obstaculo.CambiarTrayectoria(transform.position.x, transform.position.y, transform.position.z,
                                        newX, newY, newZ, newDistance);
        }
    }
}
