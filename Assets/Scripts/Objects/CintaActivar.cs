using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CintaActivar : MonoBehaviour {

    public Cinta cinta;

    bool done = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!done)
        {
            done = true;
            if (other.GetComponent<PlayerController>() != null)
            {
                cinta.Activar();
            }
            transform.position -= new Vector3(0, 0.2f, 0);

        }
    }
    public void ResetActivador()
    {
        if(done)
        {
            done = false;
            transform.position += new Vector3(0, 0.2f, 0);
        }
    }
}
