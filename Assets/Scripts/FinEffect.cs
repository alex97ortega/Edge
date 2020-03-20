using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinEffect : MonoBehaviour {
    
    bool anim = false;
    public float vel;
    PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update () {
        if (anim)
        {
            Vector3 playerpos = player.transform.position;
            player.transform.position = new Vector3(playerpos.x, playerpos.y + vel, playerpos.z);

            player.transform.Rotate(0, vel*50, 0);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().Fin();
            anim = true;
        }
    }
}
