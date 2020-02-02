using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorVel : MonoBehaviour {

    public int x, z;
    public float velocity;
    public float distance;

    bool activado = false;
    float cont = 0;
    PlayerController player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (activado)
        {
            cont += velocity;
            player.gameObject.transform.position += new Vector3(velocity * x, 0, velocity * z);
            if (cont >= distance)
            {
                cont = 0;
                activado = false;
            }
        }
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
            activado = true;
    }
}
