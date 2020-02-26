using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// objeto que se activa desde fuera y le van llamando cada vez que 
// tiene que hacer un nuevo paso
public class ActivablePorPasos : MonoBehaviour {

    public int[] posicionesPasosX;
    public int[] posicionesPasosZ;

    private uint pasos;
    private Vector3 initalPos;
    private Transform tf;
    private float nextPosX;
    private float nextPosZ;
    private float velocity = 10; // para quitarnos de movidas, todos a la misma velocidad
    private int velocidadActual; // 0 si no se mueve, 1 y -1 para distinguir dirección


    private void Start()
    {
        pasos = 0;
        velocidadActual = 0;
        tf = GetComponent<Transform>();
        initalPos = tf.position;
        nextPosX = tf.position.x;
        nextPosZ = tf.position.z;
        
    }

    // esta es la llamada que se hace desde fuera
    public void Step()
    {
        // me aseguro de que no cambie hasta que haya terminado la transformación anterior
        if (velocidadActual != 0)
            return;

        pasos++;
        // si no ha llegado al final, damos un paso mas
        // puede que haya 0 posiciones en X o en Z porque sólo nos interesa
        // mover en un eje, así que los tratamos por separado
        if(pasos <= posicionesPasosX.Length)
        {
            nextPosX = posicionesPasosX[pasos - 1];
        }
        if (pasos <= posicionesPasosZ.Length)
        {
            nextPosZ = posicionesPasosZ[pasos - 1];
        }
    }

    public void ResetObject()
    {
        pasos = 0;
        tf.position = initalPos;
        nextPosX = initalPos.x;
        nextPosZ = initalPos.z;
    }

    private void Update()
    {
        // mover en X
        if(tf.position.x != nextPosX)
        {
            MueveX();
        }

        // mover en Z
        if (tf.position.z != nextPosZ)
        {
            MueveZ();
        }
    }

    private void MueveX()
    {
        // vemos qué dirección va a tomar si es la primera vuelta
        if(velocidadActual == 0)
        {
            if (tf.position.x < nextPosX)
                velocidadActual = 1;
            else
                velocidadActual = -1;
        }
        // movemos el objeto
        else
        {
            float vel = velocity * velocidadActual* Time.deltaTime;
            tf.position += new Vector3(vel ,0, 0);

            // comprobamos si ha llegado a su destino
            if((velocidadActual == 1 && tf.position.x >= nextPosX) ||
               (velocidadActual == -1 && tf.position.x <= nextPosX))
            {
                tf.position = new Vector3(nextPosX, tf.position.y, tf.position.z);
                velocidadActual = 0;
            }
        }
    }
    private void MueveZ()
    {
        // vemos qué dirección va a tomar si es la primera vuelta
        if (velocidadActual == 0)
        {
            if (tf.position.z < nextPosZ)
                velocidadActual = 1;
            else
                velocidadActual = -1;
        }
        // movemos el objeto
        else
        {
            float vel = velocity * velocidadActual * Time.deltaTime;
            tf.position += new Vector3(0, 0, vel);

            // comprobamos si ha llegado a su destino
            if ((velocidadActual == 1 && tf.position.z >= nextPosZ) ||
               (velocidadActual == -1 && tf.position.z <= nextPosZ))
            {
                tf.position = new Vector3(tf.position.x, tf.position.y, nextPosZ);
                velocidadActual = 0;
            }
        }
    }
}
