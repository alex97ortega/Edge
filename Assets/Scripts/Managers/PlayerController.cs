using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //variables públicas
    public int velocity;
    public enum Estado
    {
        parado,
        movW, movA, movS, movD,
        subeW, subeA, subeS, subeD,
        cayendo,
        fin
    }

    //variables privadas
    Transform tf;
    int cont;    
    Estado estado;
    bool canMove;
    float x, y,z;
    bool necesitaAjuste = false;
    bool activado = true;
    

    // Use this for initialization
    void Start () {
        tf = GetComponent<Transform>();
        cont = 0;
        estado = Estado.cayendo;
        canMove = false;
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
                if (CanMove(0, 1,out est))                
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
        /////////
        switch (estado)
        {
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
                Sube();
                break;
            case Estado.cayendo:
                tf.position = new Vector3(tf.position.x, tf.position.y - 0.5f, tf.position.z);
                RaycastHit hit;
                if (Physics.Raycast(tf.position, new Vector3(0, -1, 0),out hit, 1))
                {
                    // cae en suelo
                    if (hit.collider.tag != "item" && hit.collider.tag != "trigger" && hit.collider.tag != "deathzone")
                    {
                        canMove = true;
                        estado = Estado.parado;
                        tf.position = new Vector3(tf.position.x, Mathf.Ceil(tf.position.y), tf.position.z);
                        Ajusta();
                    }
                }
                break;
            case Estado.fin:
                // estado de fin de nivel, efecto de que sube para arriba girando unos instantes
                Vector3 playerpos = transform.position;
                transform.position = new Vector3(playerpos.x, playerpos.y + 0.05f, playerpos.z);

                transform.Rotate(0, 2, 0);
                break;
            default:
                break;
        }
        if (necesitaAjuste) Ajusta();
    }
    ////////////////
    private bool CanMove(int x, int z, out Estado est)
    {
        RaycastHit hit;
        if      (x == 1)  est = Estado.movD;
        else if (x == -1) est = Estado.movA;
        else if (z == 1)  est = Estado.movW;
        else              est = Estado.movS;

        //hit para arriba, para el caso en el que haya un bloque justo encima
        if (Physics.Raycast(tf.position, new Vector3(0, 1, 0), out hit, 2)) return false;
        //hit para arriba, para el caso en el que haya un bloque justo encima en la dirección que vamos
        if (Physics.Raycast(new Vector3(tf.transform.position.x, tf.transform.position.y + 2, tf.transform.position.z),
                    new Vector3(x, 0, z), 2)) return false;


        bool hitted = Physics.Raycast(tf.position, new Vector3(x, 0, z), out hit, 2);
        if (hitted)
        {
            if (hit.collider.tag == "trigger" ||
                hit.collider.tag == "item")
                return true;
            //escalon, hay que comprobar que no hay obstaculos encima
            if (hit.collider.tag == "escalon")
            {
                if (!Physics.Raycast(
                    new Vector3(tf.transform.position.x, tf.transform.position.y + 2, tf.transform.position.z),
                    new Vector3(x, 0, z), 2) &&
                    !Physics.Raycast(
                    new Vector3(tf.transform.position.x, tf.transform.position.y + 4, tf.transform.position.z),
                    new Vector3(x, 0, z), 2))
                {

                    // También hay que comprobar el caso de que haya un bloque justo detrás, 
                    // ya que físicamente no tendría espacio para moverse

                    if (x == 1)
                    {
                        if (Physics.Raycast(tf.position, new Vector3(-1, 0, 0), out hit, 2)) return false;
                        est = Estado.subeD;
                    }
                    else if (x == -1)
                    {
                        if (Physics.Raycast(tf.position, new Vector3(1, 0, 0), out hit, 2)) return false;
                        est = Estado.subeA;
                    }
                    else if (z == 1)
                    {
                        if (Physics.Raycast(tf.position, new Vector3(0, 0, -1), out hit, 2)) return false;
                        est = Estado.subeW;
                    }
                    else
                    {
                        if (Physics.Raycast(tf.position, new Vector3(0, 0, 1), out hit, 2)) return false;
                        est = Estado.subeS;
                    }
                    return true;
                }
            }
        }
        return !hitted;
    }
    // guardamos las posiciones actuales previas al movimiento
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
    // movimiento normal hacia las 4 direcciones
    private void Rot()
    {
        cont += velocity;
        switch (estado)
        {
            case Estado.movW:
                tf.RotateAround(new Vector3(0, y - 1, z + 1), new Vector3(1, 0, 0), velocity);
                break;
            case Estado.movA:
                tf.RotateAround(new Vector3(x - 1, y - 1, 0), new Vector3(0, 0, 1), velocity);
                break;
            case Estado.movS:
                tf.RotateAround(new Vector3(0, y - 1, z - 1), new Vector3(-1, 0, 0), velocity);
                break;
            case Estado.movD:
                tf.RotateAround(new Vector3(x + 1, y - 1, 0), new Vector3(0, 0, -1), velocity);
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
    // movimiento de subida 1 posicion
    private void Sube()
    {
        cont += velocity;
        switch (estado)
        {
            case Estado.subeW:
                tf.RotateAround(new Vector3(0, y + 1, z + 1), new Vector3(1, 0, 0), velocity);
                break;
            case Estado.subeA:
                tf.RotateAround(new Vector3(x - 1, y + 1, 0), new Vector3(0, 0, 1), velocity);
                break;
            case Estado.subeS:
                tf.RotateAround(new Vector3(0, y + 1, z - 1), new Vector3(-1, 0, 0), velocity);
                break;
            case Estado.subeD:
                tf.RotateAround(new Vector3(x + 1, y + 1, 0), new Vector3(0, 0, -1), velocity);
                break;
            default:
                break;
        }
       
        if (cont == 180)
        {
            estado = Estado.parado;
            cont = 0;
        }
    }
  
    // funcion para ajustar la posicion en caso de que no esté en los tiles que corresponda
    public void Ajusta()
    {
        if (estado != Estado.parado)
        {
            necesitaAjuste = true;
            return;
        }
        float newposX = tf.position.x;
        float newposZ = tf.position.z;

        float desajusteX = newposX;
        float desajusteZ = newposZ;
         // x
        if (tf.position.x < 0)
        {
            while (desajusteX <= -2)
            {
                desajusteX += 2;
            }
        }
        else
        {
            while (desajusteX >= 2)
            {
                desajusteX -= 2;
            }
        }
        // z
        if (tf.position.z <= 0)
        {
            while (desajusteZ <= -2)
            {
                desajusteZ += 2;
            }
        }
        else
        {
            while (desajusteZ >= 2)
            {
                desajusteZ -= 2;
            }
        }
        if      (desajusteX >  1)  desajusteX =   2 - desajusteX;
        else if (desajusteX < -1)  desajusteX =   2 + desajusteX;
        if      (desajusteZ >  1)  desajusteZ =   2 - desajusteZ;
        else if (desajusteZ < -1)  desajusteZ =   2 + desajusteZ;

        newposX -= desajusteX;
        newposZ -= desajusteZ;
        tf.position = new Vector3(newposX, tf.position.y, newposZ);
        necesitaAjuste = false;
    }
    
    public void Stop()
    {
        canMove = false;
        estado = Estado.parado;
    }
    public void Fin()
    {
        Stop();
        estado = Estado.fin;
    }
    public Estado GetEstado() { return estado; }

    // entrar/ salir de estado mini controller
    public void TriggerMiniController(bool reduce)
    {
        if (activado && reduce)
        {
            activado = false;
            GetComponent<PlayerMiniController>().Activar();
        }
        else if(!activado && !reduce)
        {
            // no se pone el activado a true hasta que haya terminado de hacer
            // el efecto de que se hace grande
            GetComponent<PlayerMiniController>().Desactivar();
        }
    }
    public void Activar() { activado = true; }
}
