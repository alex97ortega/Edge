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
    

    // Use this for initialization
    void Start () {
        canMove = false;
        activado = false;
        tf = GetComponent<Transform>();
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
                if (!Physics.Raycast(tf.position, new Vector3(0, -1, 0), 2))
                    estado = Estado.cayendo;
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
                break;
            case Estado.cayendo:
                tf.position = new Vector3(tf.position.x, tf.position.y - 0.5f, tf.position.z);
                RaycastHit hit;
                if (Physics.Raycast(tf.position, new Vector3(0, -1, 0), out hit, 1))
                {
                    // cae en suelo
                     if (hit.collider.tag != "item" && hit.collider.tag != "trigger")
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
        

        bool hitted = Physics.Raycast(tf.position, new Vector3(x, 0, z), out hit, 2);
        if (hitted)
        {
            if (hit.collider.tag == "trigger" ||
                hit.collider.tag == "item")
                return true;
            //escalon, hay que comprobar que no hay obstaculos encima
            if (hit.collider.tag == "escalon")
            {                
                return false;
            }
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
            estado = Estado.parado;
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
        tf.localScale = new Vector3(tf.localScale.x * 0.95f, tf.localScale.y * 0.95f, tf.localScale.z * 0.95f);
        tf.position = new Vector3(tf.position.x, tf.position.y-0.025f, tf.position.z);

        if (tf.localScale.x <= 0.666f)
        {
            tf.localScale = new Vector3(0.666f, 0.666f, 0.666f);
            tf.position = new Vector3(tf.position.x, initialY-0.67f, tf.position.z);
            estado = Estado.parado;
            canMove = true;
        }
    }
    private void AlargaCubo()
    {
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
}
