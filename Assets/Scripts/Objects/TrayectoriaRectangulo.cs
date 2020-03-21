using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayectoriaRectangulo : MonoBehaviour {

    public float velocity;
    public float distanciaUp, distanciaDown, distanciaLeft, distanciaRight;
    public int primerMovimiento;
    public bool pararAMitad;
    public bool girarObstaculo;
    public bool esquina;

    public enum Estados
    {
        Down,
        Right,
        Up,
        Left
    }

    private Estados estado;
    private float cont, timeCont;
    private float timeReposo = 0.5f;
    private bool haParado;
    private Transform tf;

    // Use this for initialization
    void Start () {
        // en el mismo orden que están las variables públicas de distancia, 
        // ya que se va a indicar desde fuera
        switch (primerMovimiento)
        {
            case 0:
                estado = Estados.Up;
                break;
            case 1:
                estado = Estados.Down;
                break;
            case 2:
                estado = Estados.Left;
                break;
            default:
                estado = Estados.Right;
                break;
        }
        cont = 0;
        timeCont = 0;
        tf = GetComponent<Transform>();

        if (pararAMitad)
        {
            distanciaDown /= 2;
            distanciaUp /= 2;
            haParado = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        
        float vel = velocity * Time.deltaTime;

        switch (estado)
        {
            case Estados.Down:
                if (cont >= distanciaDown)
                {
                    //está unos instantes en reposo antes del cambio
                    timeCont += Time.deltaTime;
                    if(timeCont >= timeReposo)
                    {
                        transform.position = new Vector3(Mathf.Round(tf.position.x), tf.position.y, tf.position.z);

                        if (girarObstaculo)
                        {
                            foreach (var x in GetComponentsInChildren<Platform>())
                                x.DisAttach(true);
                            transform.Rotate(new Vector3(0, -90, 0));
                        }

                        timeCont = 0;
                        cont = 0;
                        // hacemos que pare a medio camino para que se pueda mover mejor el player
                        if(pararAMitad)
                        {
                            if(haParado)
                                estado = Estados.Right;
                            haParado = !haParado;
                        }
                        else
                        {
                            if(esquina)
                                estado = Estados.Up;
                            else
                                estado = Estados.Right;
                        }
                    }
                }
                else
                {
                    cont += vel;
                    transform.position += new Vector3(-vel, 0, 0);
                }
                break;
            case Estados.Right:
                if (cont >= distanciaRight)
                {
                    timeCont += Time.deltaTime;
                    if (timeCont >= timeReposo)
                    {
                        transform.position = new Vector3(tf.position.x, tf.position.y, Mathf.Round(tf.position.z));

                        if (girarObstaculo)
                        {
                            foreach (var x in GetComponentsInChildren<Platform>())
                                x.DisAttach(true);
                            transform.Rotate(new Vector3(0, -90, 0));
                        }

                        timeCont = 0;
                        cont = 0;
                        if (esquina)
                            estado = Estados.Left;
                        else
                            estado = Estados.Up;
                    }
                }
                else
                {
                    cont += vel;
                    transform.position += new Vector3(0, 0, -vel);
                }
                break;
            case Estados.Up:
                if (cont >= distanciaUp)
                {
                    timeCont += Time.deltaTime;
                    if (timeCont >= timeReposo)
                    {
                        transform.position = new Vector3(Mathf.Round(tf.position.x), tf.position.y, tf.position.z);

                        if (girarObstaculo)
                        {
                            foreach (var x in GetComponentsInChildren<Platform>())
                                x.DisAttach(true);
                            transform.Rotate(new Vector3(0, -90, 0));
                        }

                        timeCont = 0;
                        cont = 0;
                        
                        if (pararAMitad)
                        {
                            if (haParado)
                                estado = Estados.Left;
                            haParado = !haParado;
                        }
                        else
                        {
                            if (esquina)
                                estado = Estados.Right;
                            else
                                estado = Estados.Left;
                        }

                    }
                }
                else
                {
                    cont += vel;
                    transform.position += new Vector3(vel, 0, 0);
                }
                break;
            case Estados.Left:
                if (cont >= distanciaLeft)
                {
                    transform.position = new Vector3(tf.position.x, tf.position.y, Mathf.Round(tf.position.z));

                    timeCont += Time.deltaTime;
                    if (timeCont >= timeReposo)
                    {
                        float desajuste = cont - distanciaLeft;
                        transform.position += new Vector3(0, 0, desajuste);

                        if (girarObstaculo)
                        {
                            foreach (var x in GetComponentsInChildren<Platform>())
                                x.DisAttach(true);
                            transform.Rotate(new Vector3(0, -90, 0));
                        }

                        timeCont = 0;
                        cont = 0;
                        estado = Estados.Down;
                    }
                }
                else
                {
                    cont += vel;
                    transform.position += new Vector3(0, 0, vel);
                }
                break;
        }
	}
}
