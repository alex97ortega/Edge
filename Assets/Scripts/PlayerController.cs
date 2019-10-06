using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //provisional

public class PlayerController : MonoBehaviour {

    public int velocity;
    GameManager gameManager;//provisional
    Transform tf;
    int cont;
    enum Estado
    {
        parado,
        movW, movA, movS, movD,
        subeW, subeA, subeS, subeD,
        cayendo,
        fin
    }
    Estado estado;
    bool canMove;
    float x, y,z;
    Vector3 initialPos;
    //para subidas
    float guardaX, guardaZ;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        tf = GetComponent<Transform>();
        cont = 0;
        estado = Estado.cayendo;
        canMove = false;
        initialPos = tf.position;
    }
	
	// Update is called once per frame
	void Update () {       

        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                RaycastHit hit;
                if (Physics.Raycast(tf.position, new Vector3(0, 0, 1), out hit, 2))
                {
                    if (hit.collider.tag == "trigger") MoveW(Estado.movW);
                    //escalon, comprobar que no hay obstaculo encima
                    if (hit.collider.tag == "escalon")
                    {
                        if(!Physics.Raycast(
                            new Vector3(tf.transform.position.x, tf.transform.position.y+2, tf.transform.position.z),
                            new Vector3(0, 0, 1),  2))
                            MoveW(Estado.subeW);
                    }                        
                }
                else  MoveW(Estado.movW);
                
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                RaycastHit hit;
                if (Physics.Raycast(tf.position, new Vector3(-1, 0, 0), out hit, 2))
                {
                    if (hit.collider.tag == "trigger") MoveA(Estado.movA);
                    //escalon
                    if (hit.collider.tag == "escalon")
                    {
                        if (!Physics.Raycast(
                            new Vector3(tf.transform.position.x, tf.transform.position.y + 2, tf.transform.position.z),
                            new Vector3(-1, 0, 0), 2))
                            MoveA(Estado.subeA);
                    }
                }
                else MoveA(Estado.movA);
                
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                RaycastHit hit;
                if (Physics.Raycast(tf.position, new Vector3(0, 0, -1),out hit, 2))
                {
                    if (hit.collider.tag == "trigger") MoveS(Estado.movS);
                    //escalon
                    if (hit.collider.tag == "escalon")
                    {
                        if (!Physics.Raycast(
                            new Vector3(tf.transform.position.x, tf.transform.position.y + 2, tf.transform.position.z),
                            new Vector3(0, 0, -1), 2))
                            MoveS(Estado.subeS);
                    }
                }
                else MoveS(Estado.movS);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RaycastHit hit;
                if (Physics.Raycast(tf.position, new Vector3(1, 0, 0), out hit, 2))
                {
                    if (hit.collider.tag == "trigger") MoveD(Estado.movD);
                    //escalon
                    if (hit.collider.tag == "escalon")
                    {
                        if (!Physics.Raycast(
                            new Vector3(tf.transform.position.x, tf.transform.position.y + 2, tf.transform.position.z),
                            new Vector3(1, 0, 0), 2))
                            MoveD(Estado.subeD);
                    }
                }
                else MoveD(Estado.movD);
                
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
                tf.position = new Vector3(tf.position.x, tf.position.y - 0.3f, tf.position.z);
                RaycastHit hit;
                if (Physics.Raycast(tf.position, new Vector3(0, -1, 0),out hit, 1))
                {
                    if (hit.collider.tag == "deathzone")
                    {
                        if (gameManager) gameManager.ResetNivel();
                        else SceneManager.LoadScene("Nivel2");//provisional
                        /*transform.position = initialPos;
                        tf.rotation = new Quaternion(0, 0, 0, 0);
                        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);*/
                    }
                    else if (hit.collider.tag != "item" && hit.collider.tag != "trigger")
                    {
                        canMove = true;
                        estado = Estado.parado;
                        tf.position = new Vector3(tf.position.x, Mathf.Ceil(tf.position.y), tf.position.z);                       
                    }
                }
                break;
            case Estado.fin:
                Vector3 playerpos = transform.position;
                transform.position = new Vector3(playerpos.x, playerpos.y + 0.05f, playerpos.z);

                transform.Rotate(0, 2, 0);
                break;
            default:
                break;
        }
    }
   
    private void MoveW(Estado est)
    {
        if (estado==Estado.parado)
        {
            z = tf.position.z;
            y = tf.position.y;
            guardaZ = z;
            estado = est;
        }
    }
    private void MoveA(Estado est)
    {
        if (estado == Estado.parado)
        {
            x = tf.position.x;
            y = tf.position.y;
            guardaX = x;
            estado = est;
        }
    }
    private void MoveS(Estado est)
    {
        if (estado == Estado.parado)
        {
            z = tf.position.z;
            y = tf.position.y;
            guardaZ = z;
            estado = est;
        }
    }    
    private void MoveD(Estado est)
    {
        if (estado == Estado.parado)
        {
            x = tf.position.x;
            y = tf.position.y;
            guardaX = x;
            estado = est;
        }
    }
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
            // correcciones posicion
            // de momento parece que no hacen falta
            /*if (rotW)
                tf.position = new Vector3(tf.position.x, tf.position.y, guardaZ + 2);
            else if (rotA)
                tf.position = new Vector3(guardaX - 2, tf.position.y, tf.position.z);
            else if (rotS)
                tf.position = new Vector3(tf.position.x, tf.position.y, guardaZ - 2);
            else if (rotD)
                tf.position = new Vector3(guardaX + 2, tf.position.y, tf.position.z);*/
            estado = Estado.parado;
            cont = 0;
        }
    }
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
            // correcciones de subida
            switch (estado)
            {
                case Estado.subeW:
                    tf.position = new Vector3(tf.position.x, tf.position.y, guardaZ + 2);
                    break;
                case Estado.subeA:
                    tf.position = new Vector3(guardaX - 2, tf.position.y, tf.position.z);
                    break;
                case Estado.subeS:
                    tf.position = new Vector3(tf.position.x, tf.position.y, guardaZ - 2);
                    break;
                case Estado.subeD:
                    tf.position = new Vector3(guardaX + 2, tf.position.y, tf.position.z);
                    break;
                default:
                    break;
            }
            estado = Estado.parado;
            cont = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // solo podemos empezar a movernos cuando el cubo cae al suelo
        canMove = true;
        // corrección por las jodidas unidades de unity
        //tf.position = new Vector3(initialPos.x, tf.position.y, initialPos.z);
    }

    // funcion para ajustar la posicion en caso de que no esté en los tiles que corresponda
    public void Ajusta()
    {
        float desajusteX = (tf.position.x - initialPos.x) % 2;
        float desajusteZ = (tf.position.z - initialPos.z) % 2;
        if (desajusteX > 1) desajusteX =  desajusteX-2;
        if (desajusteZ > 1) desajusteZ =  desajusteZ-2;
        tf.position -= new Vector3(desajusteX, 0, desajusteZ);
        Debug.Log("x: " + desajusteX);
        Debug.Log("z: " + desajusteZ);
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
}
