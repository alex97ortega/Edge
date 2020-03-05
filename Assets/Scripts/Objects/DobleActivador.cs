using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DobleActivador : MonoBehaviour {

    public bool activado;

    public DobleActivador other;
    public ActivablePorPasos[] activateObjects;

    bool fuePrimerActivado;
    Vector3 initialPos;

    private void Start()
    {
        if (other.EstaActivado() || !activado)
            Desactivar();
        fuePrimerActivado = activado;
        initialPos = transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activado && other.GetComponent<PlayerController>() != null)
            Activar(true);
    }

    public void Activar(bool incrementa)
    {
        // puede haber opción de activarlo sin incrementar pasos, por ejemplo al resetear
        if (!incrementa)
            Activa();
        
        else
        {
            // primero pregunto si todos los objetos están en reposo
            // si hay alguno que aún se esté moviendo del paso anterior, no activo
            bool puedeActivarse = true;

            foreach (var o in activateObjects)
            {
                if (!o.CanStep())
                    puedeActivarse = false;
            }
            if(puedeActivarse)
            {
                // Incrementa un paso a cada uno de los objetos
                foreach (var o in activateObjects)
                    o.Step();
                Activa();
            }
        }
    }
    private void Activa()
    {
        activado = true;
        transform.position -= new Vector3(0, 0.3f, 0);
        other.Desactivar();
    }
    public void Desactivar()
    {
        activado = false;
        transform.position += new Vector3(0, 0.3f, 0);
    }

    public bool EstaActivado() { return activado; }
    public void ResetActivador() {
        activado = fuePrimerActivado;
        transform.localPosition = initialPos;
    }
}
