using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// cambia la trayectoria de un obstaculo cuando haya concluido su anterior
public class CambiarTrayectoria : MonoBehaviour {
    
    public int newX, newY, newZ;
    public float newDistance;

    private void Update()
    {
        GetComponent<Obstaculo>().CambiarTrayectoria(newX, newY, newZ, newDistance);
    }
}
