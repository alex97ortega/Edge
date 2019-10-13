﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int velocity;
    Transform tf;
    GameManager gameManager;
    int cont;
    public enum Estado
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
    float guardaX, guardaZ;

	// Use this for initialization
	void Start () {
        tf = GetComponent<Transform>();
        gameManager = FindObjectOfType<GameManager>();
        cont = 0;
        estado = Estado.cayendo;
        canMove = false;
    }
	
	// Update is called once per frame
	void Update () {       

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
                tf.position = new Vector3(tf.position.x, tf.position.y - 0.3f, tf.position.z);
                RaycastHit hit;
                if (Physics.Raycast(tf.position, new Vector3(0, -1, 0),out hit, 1))
                {
                    if (hit.collider.tag == "deathzone")
                    {
                        transform.position = new Vector3(guardaX, 10, guardaZ);
                        if (gameManager) gameManager.Death();
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
    ////////////////
    private bool CanMove(int x, int z, out Estado est)
    {
        RaycastHit hit;
        if      (x == 1)  est = Estado.movD;
        else if (x == -1) est = Estado.movA;
        else if (z == 1)  est = Estado.movW;
        else              est = Estado.movS;
        bool hitted = Physics.Raycast(tf.position, new Vector3(x, 0, z), out hit, 2);
        if (hitted)
        {
            if (hit.collider.tag == "trigger")
                return true;
            //escalon, comprobar que no hay obstaculo encima
            if (hit.collider.tag == "escalon")
            {
                if (!Physics.Raycast(
                    new Vector3(tf.transform.position.x, tf.transform.position.y + 2, tf.transform.position.z),
                    new Vector3(x, 0, z), 2))
                {
                    if (x == 1)       est = Estado.subeD;
                    else if (x == -1) est = Estado.subeA;
                    else if (z == 1)  est = Estado.subeW;
                    else              est = Estado.subeS;
                    return true;
                }
            }
        }
        return !hitted;
    }
    private void Move(Estado est)
    {
        if (estado == Estado.parado)
        {
            x = tf.position.x;
            y = tf.position.y;
            z = tf.position.z;
            guardaX = x;
            guardaZ = z;
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
            estado = Estado.parado;
            cont = 0;
        }
    }
  
    // funcion para ajustar la posicion en caso de que no esté en los tiles que corresponda
    public void Ajusta()
    {
        float newposx = guardaX;
        float newposz = guardaZ;
        switch (estado)
        {
            case Estado.movW:
            case Estado.subeW:
                newposz +=2;
                break;
            case Estado.movA:
            case Estado.subeA:
                newposx -= 2;
                break;
            case Estado.movS:
            case Estado.subeS:
                newposz -= 2;
                break;
            case Estado.movD:
            case Estado.subeD:
                newposx += 2;
                break;
            default:
                break;
         }
         tf.position = new Vector3(newposx, tf.position.y, newposz);
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
}
