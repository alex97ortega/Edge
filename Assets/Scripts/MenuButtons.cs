using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour {
    

    // funciones auxiliares para que llamen los botones, dado que
    // no se puede referenciar el GameManager

    public void StartTutorial()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.StartTutorial();
    }
    public void StartExperiment()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.StartExperiment();
    }
    public void Quit()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.Quit();
    }
    public void ChangeUser()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.ReturnToSessionMenu();
    }
}
