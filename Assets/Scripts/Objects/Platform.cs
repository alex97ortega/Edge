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
            /* 
            // aun no se como solucionar cuando empuja al jugador mientras no está parado, 
            // acaba el movimiento y llega a atravesar la plataforma
            if (isLateral)
                other.GetComponent<PlayerController>().EmpujadoPorPlataforma();
            */
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            // puede darse el caso de que cuando ya haya salido del todo pertenezca ya a 
            // otra plataforma. si es asi, no le quito el parent
            if (other.gameObject.transform.parent == gameObject.transform.parent)
                other.gameObject.transform.parent = null;
        }
    }
    public void DisAttach()
    {
        if (isLateral)
            GetComponentInParent<DisAttach>().DisAttachPlayer();
    }
}
