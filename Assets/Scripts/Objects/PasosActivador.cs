﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// activa la trayectoria de un Activable por Pasos si el player está encima
public class PasosActivador : MonoBehaviour {

    public ActivablePorPasos[] activateObjects;
    private bool acabaDeActivarse = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
            Activar();
    }
    public void Activar()
    {
        // damos un frame de respiro para que no active varios
        // pasos a la vez por el triggerStay
        if(acabaDeActivarse)
        {
            acabaDeActivarse = false;
            return;
        }

        // luego pregunto si todos los objetos están en reposo
        // si hay alguno que aún se esté moviendo del paso anterior, no activo
        bool puedeActivarse = true;

       foreach (var o in activateObjects)
       {
           if (!o.CanStep())
               puedeActivarse = false;
       }
       if (puedeActivarse)
       {
           // Incrementa un paso a cada uno de los objetos
           foreach (var o in activateObjects)
               o.Step();
            acabaDeActivarse = true;
       }        
    }
}
