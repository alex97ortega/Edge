using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayectoriaRectangulo2 : MonoBehaviour {

    public float velocity;
    public float distanciaUp, distanciaDown, distanciaLeft, distanciaRight;
    public int primerMovimiento;
    public bool enX;

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
                        
                        timeCont = 0;
                        cont = 0;
                      
                        estado = Estados.Left;
                    }
                }
                else
                {
                    cont += vel;
                    transform.position += new Vector3(0, -vel, 0);
                }
                break;
            case Estados.Right:
                if (cont >= distanciaRight)
                {
                    Ajusta();

                    timeCont += Time.deltaTime;
                    if (timeCont >= timeReposo)
                    {             
                        timeCont = 0;
                        cont = 0;
                        
                        estado = Estados.Down;
                    }
                }
                else
                {
                    cont += vel;
                    if(enX)
                        transform.position += new Vector3(-vel, 0, 0);
                    else
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
                        timeCont = 0;
                        cont = 0;
                        
                        estado = Estados.Right;
                    }
                }
                else
                {
                    cont += vel;
                    transform.position += new Vector3(0, vel, 0);
                }
                break;
            case Estados.Left:
                if (cont >= distanciaLeft)
                {
                    Ajusta();

                    timeCont += Time.deltaTime;
                    if (timeCont >= timeReposo)
                    {       
                        timeCont = 0;
                        cont = 0;
                        estado = Estados.Up;
                    }
                }
                else
                {
                    cont += vel;
                    if (enX)
                        transform.position += new Vector3(vel, 0, 0);
                    else
                        transform.position += new Vector3(0, 0, vel);
                }
                break;
        }
	}

    private void Ajusta()
    {
        if(tf.localScale.x != 2 || tf.localScale.z != 2)
        {
            transform.position = new Vector3(Mathf.Round(tf.position.x), Mathf.Round(tf.position.y), Mathf.Round(tf.position.z));
            return;
        }

        // para cuando sea un 2x2x2, nos aseguramos que acaba exactamente en una casilla par, para más precisión

        float auxX = Mathf.Round(tf.position.x);
        float auxZ = Mathf.Round(tf.position.z);

        // tiene que ser par tanto X como Z
        if (auxX % 2 == 0)
        {
            tf.position = new Vector3(auxX, Mathf.Round(tf.position.y), tf.position.z);
        }
        // si no, le restamos o sumamos 1 y listo
        else
        {
            if ((auxX - tf.position.x) <= 0)
                tf.position = new Vector3(auxX + 1, Mathf.Round(tf.position.y), tf.position.z);
            else
                tf.position = new Vector3(auxX - 1, Mathf.Round(tf.position.y), tf.position.z);
        }

        if (auxZ % 2 == 0)
        {
            tf.position = new Vector3(tf.position.x, Mathf.Round(tf.position.y), auxZ);
        }
        else
        {
            if ((auxX - tf.position.x) <= 0)
                tf.position = new Vector3(tf.position.x, Mathf.Round(tf.position.y), auxZ + 1);
            else
                tf.position = new Vector3(tf.position.x, Mathf.Round(tf.position.y), auxZ - 1);
        }
    }
}
