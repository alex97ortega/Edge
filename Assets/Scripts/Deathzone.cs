using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathzone : MonoBehaviour {
    public LevelManager levelManager;
    public float respawnX, respawnY, respawnZ;
    public GameObject[] obstaclesToReset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            Vector3 playerResetPos = levelManager.Dead();
            other.transform.position = playerResetPos;            
        }
    }
}
