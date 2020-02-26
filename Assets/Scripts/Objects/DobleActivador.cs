using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DobleActivador : MonoBehaviour {

    public bool activado;

    public DobleActivador other;
    public ActivablePorPasos[] activateObjects;

    private void Start()
    {
        if (other.EstaActivado() || !activado)
            Desactivar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activado && other.GetComponent<PlayerController>() != null)
            Activar();
    }

    public void Activar()
    {
        activado = true;
        transform.position -= new Vector3(0, 0.3f, 0);
        other.Desactivar();

        // Incrementa un paso a cada uno de los objetos
        foreach(var o in activateObjects)
            o.Step();
    }

    public void Desactivar()
    {
        activado = false;
        transform.position += new Vector3(0, 0.3f, 0);
    }

    public bool EstaActivado() { return activado; }
}
