using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// script para coger nombre del teclado
public class StartSessionManager : MonoBehaviour {

    public Text text;

    public void StartSession()
    {
        if (text.text == "")
            return;
        PlayerPrefs.SetString("nombre", text.text);
        FindObjectOfType<GameManager>().StartSession(text.text);
    }    
}
