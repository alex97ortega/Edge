﻿using System.Collections;
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
            float vel = velocity * Time.deltaTime;
            cont += vel;
            transform.position += new Vector3(vel * x, vel * y, vel * z);
            if (cont >= distance)
            {
                float desajuste = cont - distance;
                transform.position -= new Vector3(desajuste * x, desajuste * y, desajuste * z);
                cont = 0;
                x *= -1;
                y *= -1;
                z *= -1;
                activado = false;
                recuperarse = !recuperarse;
                foreach (var x in GetComponentsInChildren<Platform>())
                    x.DisAttach();
            }
        }
        else if (recuperarse)
        {
            if (cont < 1)
            {
                foreach (var x in GetComponentsInChildren<Platform>())
                    x.DisAttach();
            }
            // ponemos -1 en el tiempo de recuperación si no queremos que se recupere
            if (tiempoRecuperacion >= 0)
            {
                cont += Time.deltaTime;
                if (cont > tiempoRecuperacion)
                {
                    activado = true;
                    cont = 0;
                }
            }
        }

        else if (tiempoAutoActivado > 0)
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
    public bool EstaRecuperandose() { return recuperarse; }
}