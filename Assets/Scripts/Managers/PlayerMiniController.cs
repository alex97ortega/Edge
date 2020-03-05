using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script que controla el player cuando es pequeño (2/3 de su tamaño original).
// se emplean bastantes cosas similares al player controller normal, pero como es un caso
// excepcional en el juego y hay cosas significativas que sí cambian decidí tenerlo
// en un script a parte

public class PlayerMiniController : MonoBehaviour {

    public int velocity;
    public LevelManager levelManager;
    public enum Estado
    {
        reduciendo, alargando,
        parado,
        movW, movA, movS, movD,
        subeW, subeA, subeS, subeD,
        escalaW, escalaA, escalaS, escalaD, // tenemos 4 estados más que escalan
        cayendo,
        fin
    }

    Transform tf;
    Estado estado;
    bool canMove;
    bool activado;
    float initialY;
    float x, y, z;
    int cont;
    float initialTime;

    // Use this for initialization
    void Start () {
        canMove = false;
        activado = false;
        tf = GetComponent<Transform>();
        initialTime = Time.time-0.2f; // para que de primeras pueda caer sin esperar 300 ms
    }
	
	// Update is called once per frame
	void Update () {

        if (!activado)
            return;
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Estado est;
                if (CanMove(0, 1, out est))
                    Move(est);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Estado est;
                if (CanMove(-1, 0, out est))
                    Move(est);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Estado est;
                if (CanMove(0, -1, out est))
                    Move(est);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Estado est;
                if (CanMove(1, 0, out est))
                    Move(est);
            }
        }
        switch (estado)
        {
            case Estado.reduciendo:
                ReduceCubo();
                break;
            case Estado.alargando:
                AlargaCubo();
                break;
            case Estado.parado:

                // me aseguro de que no se queda bloqueado en ningun momento si no 
                // está encima o siendo empujado por una plataforma
                if (gameObject.transform.parent == null)
                {
                    tf.rotation = new Quaternion(0, 0, 0, 0);
                    canMove = true;
                }

                // como con el mini controller puede estar subiendo paredes de seguido,
                // ponemos un timer de 200 milisegundos que es el tiempo que tiene que estar 
                // parado en el aire el player hasta que empiece a caer
                if (Time.time >= initialTime + 0.2f)
                {
                    if (!Physics.Raycast(tf.position, new Vector3(0, -1, 0), 0.5f))
                        estado = Estado.cayendo;
                }
                break;
            case Estado.movW:
            case Estado.movA:
            case Estado.movS:
            case Estado.movD:
                Rot();
                break;
            case Estado.subeW:
            case Estado.subeA:
            case Estado.subeS:
            case Estado.subeD:
                Sube();
                break;
            case Estado.escalaW:
            case Estado.escalaA:
            case Estado.escalaS:
            case Estado.escalaD:
                Escala();
                break;
            case Estado.cayendo:
                tf.position = new Vector3(tf.position.x, tf.position.y - 0.33f, tf.position.z);
                RaycastHit hit;
                if (Physics.Raycast(tf.position, new Vector3(0, -1, 0), out hit, 1))
                {
                    // cae en suelo
                     if (hit.collider.tag != "item" && hit.collider.tag != "trigger" && hit.collider.tag != "deathzone")
                    {
                        canMove = true;
                        estado = Estado.parado;
                        tf.position = new Vector3(tf.position.x, Mathf.Ceil(tf.position.y)-0.67f, tf.position.z);
                    }
                }
                break;
            case Estado.fin:
                break;
            default:
                break;
        }
    }
    // esto cambia con respecto a player controller.
    // ya no afectan obstaculos encima ni a los lados que antes bloqueaban
    private bool CanMove(int x, int z, out Estado est)
    {
        RaycastHit hit;
        if (x == 1) est = Estado.movD;
        else if (x == -1) est = Estado.movA;
        else if (z == 1) est = Estado.movW;
        else est = Estado.movS;

        // si hay un obstáculo justo encima del player, pues nada (teniendo en cuenta su tamaño nuevo)
        if (Physics.Raycast(tf.position, new Vector3(0, 1, 0), out hit, 0.5f)) return false;

        bool hitted = Physics.Raycast(tf.position, new Vector3(x, 0, z), out hit, 0.5f);
        if (hitted)
        {
            if (hit.collider.tag == "trigger" ||
                hit.collider.tag == "item")
                return true;

            if (hit.collider.tag == "escalon")
            {
                // si hay otro obstaculo encima, se escala
                // si no, se sube como hacía player controller

                if (Physics.Raycast(new Vector3(tf.transform.position.x, tf.transform.position.y + 0.5f, tf.transform.position.z),
                     new Vector3(x, 0, z), 0.5f))
                {
                    if (x == 1)
                        est = Estado.escalaD;
                    else if (x == -1)
                        est = Estado.escalaA;
                    else if (z == 1)
                        est = Estado.escalaW;
                    else
                        est = Estado.escalaS;
                }
                else
                {
                    if (x == 1)
                        est = Estado.subeD;
                    else if (x == -1)
                        est = Estado.subeA;
                    else if (z == 1)
                        est = Estado.subeW;
                    else
                        est = Estado.subeS;
                }
                return true;
            }
        }
        else // !hitted
        {
            // si está escalando y deja de pulsar para arriba, 
            // no podemos dejar que se mueva para los lados en el aire
            if (!Physics.Raycast(tf.position, new Vector3(0, -1, 0), 0.5f))
                return false;
        }
        return !hitted;
    }
    // igual que player controller
    private void Move(Estado est)
    {
        if (estado == Estado.parado)
        {
            x = tf.position.x;
            y = tf.position.y;
            z = tf.position.z;
            estado = est;
        }
    }
    // movimiento normal hacia las 4 direcciones, igual que player controller  
    // salvo el punto donde va a rotar
    private void Rot()
    {
        cont += velocity;
        switch (estado)
        {
            case Estado.movW:
                tf.RotateAround(new Vector3(0, y - 0.33f, z + 0.33f), new Vector3(1, 0, 0), velocity);
                break;
            case Estado.movA:
                tf.RotateAround(new Vector3(x - 0.33f, y - 0.33f, 0), new Vector3(0, 0, 1), velocity);
                break;
            case Estado.movS:
                tf.RotateAround(new Vector3(0, y - 0.33f, z - 0.33f), new Vector3(-1, 0, 0), velocity);
                break;
            case Estado.movD:
                tf.RotateAround(new Vector3(x + 0.33f, y - 0.33f, 0), new Vector3(0, 0, -1), velocity);
                break;
            default:
                break;
        }
        if (cont == 90)
        {
            AjustaXZ();
            estado = Estado.parado;
            initialTime = Time.time;
            cont = 0;
        }
    }
    // movimiento de subir escalon al igual que el player controller normal
    private void Sube()
    {
        cont += velocity;
        switch (estado)
        {
            case Estado.subeW:
                tf.RotateAround(new Vector3(0, y + 0.33f, z + 0.33f), new Vector3(1, 0, 0), velocity);
                break;
            case Estado.subeA:
                tf.RotateAround(new Vector3(x - 0.33f, y + 0.33f, 0), new Vector3(0, 0, 1), velocity);
                break;
            case Estado.subeS:
                tf.RotateAround(new Vector3(0, y + 0.33f, z - 0.33f), new Vector3(-1, 0, 0), velocity);
                break;
            case Estado.subeD:
                tf.RotateAround(new Vector3(x + 0.33f, y + 0.33f, 0), new Vector3(0, 0, -1), velocity);
                break;
            default:
                break;
        }

        if (cont == 180)
        {
            // el ajuste en Y solo lo hacemos cuando suba peldaños
            if(transform.parent == null)
                tf.position = new Vector3(tf.position.x, Mathf.Round(tf.position.y) + 0.33f, tf.position.z);

            AjustaXZ();
            estado = Estado.parado;
            initialTime = Time.time;
            cont = 0;
        }
    }
    // movimiento de subida 1 posicion
    private void Escala()
    {
        cont += velocity;
        switch (estado)
        {
            case Estado.escalaW:
                tf.RotateAround(new Vector3(0, y + 0.33f, z + 0.33f), new Vector3(1, 0, 0), velocity);
                break;
            case Estado.escalaA:
                tf.RotateAround(new Vector3(x - 0.33f, y + 0.33f, 0), new Vector3(0, 0, 1), velocity);
                break;
            case Estado.escalaS:
                tf.RotateAround(new Vector3(0, y + 0.33f, z - 0.33f), new Vector3(-1, 0, 0), velocity);
                break;
            case Estado.escalaD:
                tf.RotateAround(new Vector3(x + 0.33f, y + 0.33f, 0), new Vector3(0, 0, -1), velocity);
                break;
            default:
                break;
        }

        if (cont == 90)
        {
            estado = Estado.parado;
            initialTime = Time.time;
            cont = 0;
        }
    }
    public void Activar()
    {
        activado = true;
        estado = Estado.reduciendo;
        initialY = tf.position.y;
    }

    public void Desactivar()
    {
        estado = Estado.alargando;
    }

    private void ReduceCubo()
    {
        // me aseguro de que no esté rotado el cubo mientras se reduce o se agranda
        tf.rotation = new Quaternion(0, 0, 0, 0);
        tf.localScale = new Vector3(tf.localScale.x * 0.95f, tf.localScale.y * 0.95f, tf.localScale.z * 0.95f);
        tf.position = new Vector3(tf.position.x, tf.position.y-0.025f, tf.position.z);

        if (tf.localScale.x <= 0.666f)
        {
            tf.localScale = new Vector3((float)2/3, (float)2 /3, (float)2 /3);
            tf.position = new Vector3(tf.position.x, initialY-0.67f, tf.position.z);
            estado = Estado.parado;
            canMove = true;
        }
    }
    private void AlargaCubo()
    {
        tf.rotation = new Quaternion(0, 0, 0, 0);
        tf.localScale = new Vector3(tf.localScale.x * 1.05f, tf.localScale.y * 1.05f, tf.localScale.z * 1.05f);
        tf.position = new Vector3(tf.position.x, tf.position.y + 0.025f, tf.position.z);

        if (tf.localScale.x >= 2)
        {
            tf.localScale = new Vector3(2, 2, 2);
            //tf.position = new Vector3(tf.position.x, initialY + 0.67f, tf.position.z);
            // se ha terminado de hacer grande. Cambiamos de miniController a controller normal
            activado = false;
            GetComponent<PlayerController>().Activar();
        }
    }
    public bool MiniControllerActivated() { return activado; }

    // el cubo tarda 3 movimientos en avanzar 2 unidades y, por lo tanto, obtener una posición
    // que debería ser exacta en X o en Z. Ahora mismo, como se mueve de 0.66 en 0.66 unidades
    // esta posición nunca es exacta y en cuanto se hacen 10 o más movimientos es un caos
    // porque ya no cuadran las casillas. Hay que redondear las posiciones cuando vaya a llegar 
    // al movimiento que debería ser exacto
    private void AjustaXZ()
    {
        // esto es para que no reajuste si está encima de una plataforma
        if (transform.parent != null)
            return;

        float auxX = Mathf.Round(tf.position.x);
        float auxZ = Mathf.Round(tf.position.z);
        
        // tiene que ser par tanto X como Z
        if (auxX % 2 == 0)
        {
            tf.position = new Vector3(auxX, tf.position.y, tf.position.z);
        }

        if (auxZ % 2 == 0)
        {
            tf.position = new Vector3(tf.position.x, tf.position.y, auxZ);
        }
    }
    public void Stop()
    {
        canMove = false;
        tf.rotation = new Quaternion(0, 0, 0, 0);
        estado = Estado.parado;
    }
    public bool EstaParado() { return (estado == Estado.parado); }
}
