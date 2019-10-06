using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaculo : MonoBehaviour {

    public int x, y, z;
    public float distance;
    public float velocity;
    public float tiempoRecuperacion;
    public float tiempoAutoActivado;

    float cont = 0;
    float contAutoActivado=0;
    public bool activado = false;
    bool recuperarse = false;

    private void Update()
    {
        if (activado)
        {
            cont += velocity;
            transform.position += new Vector3(velocity * x, velocity * y, velocity * z);
            if (cont >= distance)
            {
                cont = 0;
                x *= -1;
                y *= -1;
                z *= -1;
                activado = false;
                recuperarse = !recuperarse;
            }
        }
        else if (recuperarse)
        {
            cont += Time.deltaTime;
            if (cont > tiempoRecuperacion)
            {
                activado = true;
                cont = 0;
            }
        }

        else if (tiempoAutoActivado != 0)
        {
            contAutoActivado += Time.deltaTime;
            if (contAutoActivado > tiempoAutoActivado)
            {
                Activar();
                contAutoActivado = 0;
            }
        }
    }
    public void Activar() { activado = true; }   
    public bool EstaActivado() { return activado; }       
}
