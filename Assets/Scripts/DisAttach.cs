using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisAttach : MonoBehaviour {

    public void DisAttachPlayer()
    {
        if (GetComponentInChildren<PlayerController>())
        {
            GetComponentInChildren<PlayerController>().Ajusta();
            GetComponentInChildren<PlayerController>().gameObject.transform.parent = null;
        }        
    }    
}
