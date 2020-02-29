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
        activado = true;
        transform.position -= new Vector3(0, 0.3f, 0);
        other.Desactivar();

        // puede haber opción de activarlo sin incrementar pasos, por ejemplo al resetear

        if(incrementa)
        {
            // Incrementa un paso a cada uno de los objetos
            foreach (var o in activateObjects)
                o.Step();
        }
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
