using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayectoriaRectangulo : MonoBehaviour {

    public float velocity;
    public float distanciaUp, distanciaDown, distanciaLeft, distanciaRight;
    public int primerMovimiento;
    public bool pararAMitad;
    public bool girarObstaculo;
    public bool delReves, esquina, esquina2;

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
                    Ajusta();

                    //está unos instantes en reposo antes del cambio
                    timeCont += Time.deltaTime;
                    if(timeCont >= timeReposo)
                    {
                        if (girarObstaculo)
                        {
                            foreach (var x in GetComponentsInChildren<Platform>())
                                x.DisAttach(true);

                            if (delReves)
                                transform.Rotate(new Vector3(0, 90, 0));
                            else
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
                            if(esquina || esquina2)
                                estado = Estados.Up;
                            else if (delReves)
                                estado = Estados.Left;
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
                    Ajusta();

                    timeCont += Time.deltaTime;
                    if (timeCont >= timeReposo)
                    {
                        if (girarObstaculo)
                        {
                            foreach (var x in GetComponentsInChildren<Platform>())
                                x.DisAttach(true);
                            if(delReves)
                                transform.Rotate(new Vector3(0, 90, 0));
                            else
                                transform.Rotate(new Vector3(0, -90, 0));
                        }

                        timeCont = 0;
                        cont = 0;
                        if (esquina)
                            estado = Estados.Left;
                        else if (esquina2 || delReves)
                            estado = Estados.Down;
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
                    Ajusta();

                    timeCont += Time.deltaTime;
                    if (timeCont >= timeReposo)
                    {
                        if (girarObstaculo)
                        {
                            foreach (var x in GetComponentsInChildren<Platform>())
                                x.DisAttach(true);

                            if (delReves)
                                transform.Rotate(new Vector3(0, 90, 0));
                            else
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
                            if (esquina || delReves)
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
                    Ajusta();

                    timeCont += Time.deltaTime;
                    if (timeCont >= timeReposo)
                    {
                        if (girarObstaculo)
                        {
                            foreach (var x in GetComponentsInChildren<Platform>())
                                x.DisAttach(true);

                            if (delReves)
                                transform.Rotate(new Vector3(0, 90, 0));
                            else
                                transform.Rotate(new Vector3(0, -90, 0));
                        }

                        timeCont = 0;
                        cont = 0;
                        if(esquina2)
                            estado = Estados.Right;
                        else if (delReves)
                            estado = Estados.Up;
                        else
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
    public void Ajusta()
    {
        if(tf.localScale.x != 2 || tf.localScale.z != 2)
        {
            transform.position = new Vector3(Mathf.Round(tf.position.x), tf.position.y, Mathf.Round(tf.position.z));
            return;
        }

        // para cuando sea un 2x2x2, nos aseguramos que acaba exactamente en una casilla par, para más precisión

        float auxX = Mathf.Round(tf.position.x);
        float auxZ = Mathf.Round(tf.position.z);

        // tiene que ser par tanto X como Z
        if (auxX % 2 == 0)
        {
            tf.position = new Vector3(auxX, tf.position.y, tf.position.z);
        }
        // si no, le restamos o sumamos 1 y listo
        else
        {
            if ((auxX - tf.position.x) <= 0)
                tf.position = new Vector3(auxX + 1, tf.position.y, tf.position.z);
            else
                tf.position = new Vector3(auxX - 1, tf.position.y, tf.position.z);
        }

        if (auxZ % 2 == 0)
        {
            tf.position = new Vector3(tf.position.x, tf.position.y, auxZ);
        }
        else
        {
            if ((auxX - tf.position.x) <= 0)
                tf.position = new Vector3(tf.position.x, tf.position.y, auxZ + 1);
            else
                tf.position = new Vector3(tf.position.x, tf.position.y, auxZ - 1);
        }
    }
}
