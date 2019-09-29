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
        transform.position = player.transform.position + initialPos;
    }
}
