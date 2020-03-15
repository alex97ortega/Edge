using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public LevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            levelManager.ChangeCheckpoint(this);
            GetComponent<BoxCollider>().enabled = false; // ya no lo necesitamos más, y nos quitamos de problemas
        }
    }
}
