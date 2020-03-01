using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaculo : MonoBehaviour {

    public int x, y, z;
    public float distance;
    public float velocity;
    public float tiempoRecuperacion;
    public float tiempoAutoActivado;

    
    float contAutoActivado=0;
    public bool activado = false;
    bool recuperarse = false;
    Vector3 initialpos;
    int initialX, initialY, initialZ;
    float initialDistance;
    float cont;

    private void Start()
    {
        initialpos = transform.position;
        initialX = x;
        initialY = y;
        initialZ = z;
        cont = 0;
        initialDistance = distance;
    }

    private void Update()
    {
        if (activado)
        {
            Move();
        }
        else if (recuperarse)
        {
            // ponemos -1 en el tiempo de recuperación si no queremos que se recupere
            if (tiempoRecuperacion >= 0)
            {
                cont += Time.deltaTime;
                if (cont > tiempoRecuperacion)
                {
                    activado = true;
                    cont = 0;
                }
                else
                {
                    foreach (var x in GetComponentsInChildren<Platform>())
                        x.DisAttach();
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
    public void Activar() {
        // para que no active desde fuera si ha cambiado de dirección
        //if ((x == initialX) && (y == initialY) && (z == initialZ))
        activado = true;
    }   
    public bool EstaActivado() { return activado; }      
    public bool EstaRecuperandose() { return recuperarse; }

    public void ResetObstacle()
    {
        transform.position = initialpos;
        activado = false;
        recuperarse = false;
        contAutoActivado = 0;
        cont = 0;
        x = initialX;
        y = initialY;
        z = initialZ;
        distance = initialDistance;
    }

    private void Move()
    {
        float vel = velocity * Time.deltaTime;
        
        transform.position += new Vector3(vel * x, vel * y, vel * z);

        float destino;
        bool llegadoDestino = false;

        // tratamos por separado los 3 ejes
        if (x != 0)
        {
            if (recuperarse)
                destino = initialpos.x;
            else
                destino = initialpos.x + (x * distance);

            if (x < 0)
                llegadoDestino = (transform.position.x <= destino);
            else
                llegadoDestino = (transform.position.x >= destino);
        }
        else if (y != 0)
        {
            if (recuperarse)
                destino = initialpos.y;
            else
                destino = initialpos.y + (y * distance);
            if (y < 0)
                llegadoDestino = (transform.position.y <= destino);
            else
                llegadoDestino = (transform.position.y >= destino);
        }
        else if(z!=0)
        {
            if (recuperarse)
                destino = initialpos.z;
            else
                destino = initialpos.z + (z * distance);
            if (z < 0)
                llegadoDestino = (transform.position.z <= destino);
            else
                llegadoDestino = (transform.position.z >= destino);
        }

        if (llegadoDestino)
        {
            transform.position = new Vector3(Mathf.Round(transform.position.x),
                transform.position.y, Mathf.Round(transform.position.z));
            
            x *= -1;
            y *= -1;
            z *= -1;
            activado = false;
            recuperarse = !recuperarse;
            foreach (var x in GetComponentsInChildren<Platform>())
                x.DisAttach();
        }
    }

    public void CambiarTrayectoria(float actualX,float actualY, float actualZ, int newX, int newY, int newZ, float newDistance)
    {
        // Ajusto la posición desde la que va a salir
        transform.position = new Vector3(actualX, actualY, actualZ);

        activado = true;
        recuperarse = false;
        contAutoActivado = 0;
        cont = 0;
        x = newX;
        y = newY;
        z = newZ;
        distance = newDistance;
    }
}
