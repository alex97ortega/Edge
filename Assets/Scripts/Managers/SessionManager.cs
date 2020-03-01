using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase que registra los parámetros del juego que necesitaremos para hacer el análisis
// se va llamando desde el GameManager cuando ocurre un evento notable en el juego 
public class SessionManager : MonoBehaviour {

    private int user_id;
    private float count_down;
    //private API api;

	// Use this for initialization
	void Start () {
		//api = new API();
	}

    // llamadas públicas para llamar desde el GameManager
    public void LogStartLevel()
    {

    }
    public void LogEndLevel()
    {

    }
    public void LogItemGotten()
    {

    }
    public void LogPlayerDead()
    {

    }
    
    // método privado para registrar parámetros que sean iguales para todos los eventos
	private void LogEvent()
    {

    }
}
