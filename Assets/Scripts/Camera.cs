using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    PlayerController player;
    Vector3 initialPos;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        initialPos = transform.position - player.transform.position;
    }
    private void Update()
    {
        if(player.transform.position.y >= -1)
        {
            // si el player está haciendo movimiento normal, 
            //evitamos el movimiento de la cámara en el eje Y
            if(player.NormalMoving())
            {
                transform.position = new Vector3(player.transform.position.x + initialPos.x, transform.position.y,
                                                 player.transform.position.z + initialPos.z);
            }
            else
                transform.position = player.transform.position + initialPos;
        }
    }
}
