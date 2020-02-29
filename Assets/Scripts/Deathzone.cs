using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathzone : MonoBehaviour {
    public LevelManager levelManager;
    public float respawnX, respawnY, respawnZ;
    public GameObject[] obstaclesToReset;

    private void OnTriggerEnter(Collider other)
    {
        // dependiendo de dónde se haya caido, su respawn será 
        // en un sitio o en otro
        if (other.GetComponent<PlayerController>() != null)
        {
            levelManager.Dead();
            other.transform.position = new Vector3(respawnX, respawnY, respawnZ);

            foreach (var o in obstaclesToReset)
            {
                if (o.GetComponent<Obstaculo>())
                    o.GetComponent<Obstaculo>().ResetObstacle();
                else if (o.GetComponent<ActivablePorPasos>())
                    o.GetComponent<ActivablePorPasos>().ResetObject();
                else if (o.GetComponent<DobleActivador>())
                    o.GetComponent<DobleActivador>().ResetActivador();
            }
        }
    }
}
