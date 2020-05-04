using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //variables públicas
    public enum Estado
    {
        parado,
        movW, movA, movS, movD,
        subeW, subeA, subeS, subeD,
        cayendo,
        fin
    }

    //variables privadas
    int velocity = 500;
    Transform tf;
    int cont;    
    Estado estado;
    bool canMove;
    float x, y,z;
    bool activado = true;
    Vector3 iniScale;
    

    // Use this for initialization
    void Start () {
        tf = GetComponent<Transform>();
        iniScale = tf.localScale;
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
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Estado est;
                if (CanMove(0, 1,out est))                
                    Move(est);                
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Estado est;
                if (CanMove(-1, 0, out est))
                    Move(est);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Estado est;
                if (CanMove(0, -1, out est))
                    Move(est);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
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
                {
                    // me aseguro de que no se deforma
                    if (gameObject.transform.parent == null && tf.localScale != iniScale)
                        tf.localScale = iniScale;

                    RaycastHit hit;
                    if (!Physics.Raycast(tf.position, new Vector3(0, -1, 0), out hit, 2) 
                        || hit.collider.tag == "item" || hit.collider.tag == "checkpoint")
                        estado = Estado.cayendo;
                    else
                    {
                        // para que puede reanudar el movimiento si lo para una plataforma mientras le empuja
                        if (gameObject.transform.parent == null)
                        {
                            canMove = true;
                            tf.rotation = new Quaternion(0, 0, 0, 0);
                        }

                        // provisional, para que aparezca ya reducido
                        //TriggerMiniController(true);
                    }
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
            case Estado.cayendo:
                {
                    tf.position = new Vector3(tf.position.x, tf.position.y - 0.5f, tf.position.z);
                    RaycastHit hit;
                    if (Physics.Raycast(tf.position, new Vector3(0, -1, 0), out hit, 1))
                    {
                        // cae en suelo
                        if (hit.collider.tag != "item" && hit.collider.tag != "deathzone" && hit.collider.tag != "checkpoint"
                            /*&& hit.collider.tag != "trigger"*/) // tuve que quitar esto para que pudiera caer encima de plataformas
                        {
                            canMove = true;
                            estado = Estado.parado;
                            AjustaY();
                            Ajusta();
                        }
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
        if (Physics.Raycast(tf.position, new Vector3(0, 1, 0), 2)) return false;
        //hit para arriba, para el caso en el que haya un bloque justo encima en la dirección que vamos
        if (Physics.Raycast(new Vector3(tf.transform.position.x, tf.transform.position.y + 2, tf.transform.position.z),
                    new Vector3(x, 0, z), 2)) return false;


        // hit para direccion que sea
        bool hitted = Physics.Raycast(tf.position, new Vector3(x, 0, z), out hit, 2);
        if (hitted)
        {
            if (hit.collider.tag == "trigger" ||
                hit.collider.tag == "item" || 
                hit.collider.tag == "checkpoint")
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
            FindObjectOfType<LevelManager>().IncreaseMovements();
            x = tf.position.x;
            y = tf.position.y;
            z = tf.position.z;
            estado = est;
        }
    }
    // movimiento normal hacia las 4 direcciones
    private void Rot()
    {
        cont += (int)(velocity*Time.deltaTime);
        switch (estado)
        {
            case Estado.movW:
                tf.RotateAround(new Vector3(0, y - 1, z + 1), new Vector3(1, 0, 0), (int)(velocity * Time.deltaTime));
                break;
            case Estado.movA:
                tf.RotateAround(new Vector3(x - 1, y - 1, 0), new Vector3(0, 0, 1), (int)(velocity * Time.deltaTime));
                break;
            case Estado.movS:
                tf.RotateAround(new Vector3(0, y - 1, z - 1), new Vector3(-1, 0, 0), (int)(velocity * Time.deltaTime));
                break;
            case Estado.movD:
                tf.RotateAround(new Vector3(x + 1, y - 1, 0), new Vector3(0, 0, -1), (int)(velocity * Time.deltaTime));
                break;
            default:
                break;
        }
        if (cont >= 90)
        {
            AjustaY();
            Ajusta();
            estado = Estado.parado;
            tf.rotation = new Quaternion(0, 0, 0, 0);
            cont = 0;
        }
    }
    // movimiento de subida 1 posicion
    private void Sube()
    {
        cont += (int)(velocity * Time.deltaTime);
        switch (estado)
        {
            case Estado.subeW:
                tf.RotateAround(new Vector3(0, y + 1, z + 1), new Vector3(1, 0, 0), (int)(velocity * Time.deltaTime));
                break;
            case Estado.subeA:
                tf.RotateAround(new Vector3(x - 1, y + 1, 0), new Vector3(0, 0, 1), (int)(velocity * Time.deltaTime));
                break;
            case Estado.subeS:
                tf.RotateAround(new Vector3(0, y + 1, z - 1), new Vector3(-1, 0, 0), (int)(velocity * Time.deltaTime));
                break;
            case Estado.subeD:
                tf.RotateAround(new Vector3(x + 1, y + 1, 0), new Vector3(0, 0, -1), (int)(velocity * Time.deltaTime));
                break;
            default:
                break;
        }
       
        if (cont >= 180)
        {
            AjustaY();
            Ajusta();
            estado = Estado.parado;
            tf.rotation = new Quaternion(0, 0, 0, 0);
            cont = 0;
        }
    }
  
    // funcion para ajustar la posicion en caso de que no esté en los tiles que corresponda
    public void Ajusta()
    {
        if (!activado)
            return;

        if (transform.parent != null)
            return;

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
           if((auxX - tf.position.x) <= 0)
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
    // por si acaso hay que llamar a esta a parte
    public void AjustaY()
    {
        float auxY = Mathf.Round(tf.position.y);
        if (auxY % 2 == 0)
            tf.position = new Vector3(tf.position.x, auxY, tf.position.z);
        else
        {
            if ((auxY - tf.position.y) <= 0)
                tf.position = new Vector3(tf.position.x, auxY+1, tf.position.z);
            else
                tf.position = new Vector3(tf.position.x, auxY-1, tf.position.z);
        }
    }
    public void Stop()
    {
        canMove = false;
        AjustaY();
        cont = 0;
        tf.rotation = new Quaternion(0, 0, 0, 0);
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
    public bool EstaActivado() { return activado; }
    public void ContinueMove() { canMove = true; }
}
