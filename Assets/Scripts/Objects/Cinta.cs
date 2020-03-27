using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinta : MonoBehaviour {

    public Cinta siguienteCinta;

    public enum Estados
    {
        Down,
        Right,
        Up,
    }
    Estados estado;
    bool activado = false;
    float velocity = 8;
    float distanciaUp, distanciaDown, distanciaRight, cont;
    Transform tf;
    Vector3 initialpos;

    // Use this for initialization
    void Start () {
        estado = Estados.Down;
        distanciaDown = distanciaUp = 2;
        distanciaRight = 14;
        cont = 0;
        tf = GetComponent<Transform>();
        initialpos = tf.position;
    }
	
	// Update is called once per frame
	void Update () {

        if (!activado)
            return;

        float vel = velocity * Time.deltaTime;

        switch (estado)
        {
            case Estados.Down:
                if (cont >= distanciaDown)
                {
                    transform.position = new Vector3(tf.position.x, initialpos.y-distanciaDown, tf.position.z);
                    
                    cont = 0;
                    estado = Estados.Right;
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
                    transform.position = new Vector3(Mathf.Round(tf.position.x), tf.position.y, Mathf.Round(tf.position.z));

                    cont = 0;
                    estado = Estados.Up;
                }
                else
                {
                    if (cont >= 2 && tf.position.x <= 132)
                        siguienteCinta.Activar();
                    cont += vel;
                    if (tf.position.x < 106 || tf.position.z <= -6)
                        transform.position += new Vector3(vel, 0, 0);
                    else
                        transform.position += new Vector3(0, 0, -vel);
                }
                break;
            case Estados.Up:
                if (cont >= distanciaUp)
                {
                    transform.position = new Vector3(tf.position.x, initialpos.y, tf.position.z);

                    cont = 0;
                    estado = Estados.Down;
                    activado = false;
                }
                else
                {
                    cont += vel;
                    transform.position += new Vector3(0, vel, 0);
                }
                break;
        }
    }

    public void Activar()
    {
        activado = true;
    }
    public void ResetCinta()
    {
        transform.position = initialpos;
        cont = 0;
        estado = Estados.Down;
        activado = false;
    }
}
