using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public bool isLateral;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>()!=null)
        {            
            other.gameObject.transform.parent = gameObject.transform.parent;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            other.gameObject.GetComponent<PlayerController>().Ajusta();
            other.gameObject.transform.parent = null;
        }
    }
    public void DisAttach()
    {
        if (isLateral)
            GetComponentInParent<DisAttach>().DisAttachPlayer();
    }
}
