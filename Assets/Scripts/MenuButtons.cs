using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour {

    public GameObject botonQuit;

    private void Start()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (!gm.levelButtons)
            botonQuit.SetActive(false);
    }

    // funciones auxiliares para que llamen los botones, dado que
    // no se puede referenciar el GameManager

    public void StartTutorial()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.StartTutorial();
    }
    public void Quit()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.End();
    }
}
