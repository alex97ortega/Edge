using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public bool isLateral;
    public bool stopPlayer = false;
    public bool disAttachEnPositivo = true;

    bool checkAttachPositivo = false;
    bool checkAttachNegativo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>()!=null)
        {
            other.gameObject.transform.parent = gameObject.transform.parent;

            // para plataformas en las que sea mejor opción que el player  
            // no se mueva y así evitar bugs
            if (stopPlayer)
            {
                if (other.GetComponent<PlayerController>().EstaActivado())
                    other.GetComponent<PlayerController>().Stop();
                else
                    other.GetComponent<PlayerMiniController>().Stop();
            }

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
    public void DisAttach(bool positivo)
    {
        if (isLateral)
        {
            if (disAttachEnPositivo == positivo)
                GetComponentInParent<DisAttach>().DisAttachPlayer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            if(checkAttachPositivo && disAttachEnPositivo)
            {
                checkAttachPositivo = false;
                other.gameObject.transform.parent = gameObject.transform.parent;
            }
            else if (checkAttachNegativo && !disAttachEnPositivo)
            {
                checkAttachNegativo = false;
                other.gameObject.transform.parent = gameObject.transform.parent;
            }
        }
    }
    public void CheckAttachs(bool positivo)
    {
        if (positivo)
            checkAttachPositivo = true;
        else
            checkAttachNegativo = true;
    }
}
