using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public Text textLevelTime;
    public Text textExperimentTime;

    public GameObject menuFinish;

    public Light light;
    public Checkpoint currentCheckpoint;
    public GameObject[] objectsToReset;

    float initialTime;

    GameManager gameManager;

    float lightCont = 0.5f;
    bool lightEffect = false;
    bool stop = false;

    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();

        uint totalItems = 0;
        foreach (var x in FindObjectsOfType<Item>())
            totalItems++;

        if(gameManager)
        {
            gameManager.SetTotalItems(totalItems);
            gameManager.SetInitialTimeExperiment();
        }

        initialTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        // efecto de coger un cubito
        if (lightEffect)
        {
            lightCont += 0.05f;
            light.color = new Color(lightCont, lightCont, 1);
            if (lightCont >= 1)
            {
                light.color = new Color(1, 1, 1);
                lightEffect = false;
                lightCont = 0.5f;
            }
        }
        ShowLevelTime();

        if (gameManager && textExperimentTime != null)
            ShowExperimentTime();
    }
    public void ItemGotten()
    {
        lightEffect = true;
        if(gameManager) gameManager.ItemGotten();
    }
    public void LevelPassed()
    {
        if (gameManager)
        {
            gameManager.LevelPassed(textLevelTime.text);
        }
    }

    // tiempo del nivel actual
    private void ShowLevelTime()
    {
        if (stop)
            return;
        string min, seg;
        float _time = Time.time - initialTime;
        ConvertTimeToMinSeg(_time, out min, out seg);
        textLevelTime.text = min + ":" + seg;
    }

    // tiempo desde que comenzó el experimento
    private void ShowExperimentTime()
    {

        if (stop)
            return;

        string min, seg;
        float _time = gameManager.GetExperimentTime()  
                    + gameManager.GetInitialTimeExperiment() 
                    - gameManager.GetMenuTime() 
                    - Time.time;

        // puro efecto visual para el primer nivel del experimento, 
        // para que aparezca el contador el 12:00 y no 11:59 pero en realidad
        // es 11:59. Además, si no parece que no cuadra el tiempo de cuenta atrás
        // con el de nivel debido a redondeos
        if (gameManager.GetLevel() == 4)
        {
            _time++;
            if (_time > gameManager.GetExperimentTime())
                _time = gameManager.GetExperimentTime();
        }

        // enseño el menú de fin si el contador llega a 0
        if(_time <= 0)
            EndExperiment();

        ConvertTimeToMinSeg(_time,out min, out seg);
        textExperimentTime.text = min + ":" + seg;
    }

    private void ConvertTimeToMinSeg(float _time, out string  min, out string seg)
    {
        //min
        if (_time >= 60)
        {
            if (_time >= 600)
                min = ((int)_time / 60).ToString();
            else
                min = "0" + ((int)_time / 60).ToString();
        }
        else
            min = "00";

        //seg
        if (_time % 60 == 0)
            seg = "00";
        else if (_time % 60 < 10)
            seg = "0" + ((int)_time % 60).ToString();
        else
            seg = ((int)_time % 60).ToString();
    }

    public void ChangeCheckpoint(Checkpoint newCheckp) { currentCheckpoint = newCheckp; }

    // devuelve la posicion donde tiene que resetearse el player
    public Vector3 Dead()
    {
        if (gameManager)
            gameManager.Dead();

        // reseteo las posiciones de los posibles bloques 
        // que puedan impedir el avanzar el nivel en la posición actual

        foreach (var b in FindObjectsOfType<SueloQuebradizo>())
            b.RestartBlock();

        // tambien reseteo los posibles objetos que se necesite para poder
        // pasar el nivel
        foreach (var o in objectsToReset)
        {
            if (o.GetComponent<Obstaculo>())
                o.GetComponent<Obstaculo>().ResetObstacle();
            else if (o.GetComponent<ActivablePorPasos>())
                o.GetComponent<ActivablePorPasos>().ResetObject();
            else if (o.GetComponent<DobleActivador>())
                o.GetComponent<DobleActivador>().ResetActivador();
        }

        return currentCheckpoint.transform.position;
    }

    private void EndExperiment()
    {
        stop = true;
        menuFinish.SetActive(true);

        // para que no pueda mover el player
        FindObjectOfType<PlayerController>().Stop();
    }

    public void ReturnToSessionMenu()
    {
        gameManager.ReturnToSessionMenu();
    }
}
