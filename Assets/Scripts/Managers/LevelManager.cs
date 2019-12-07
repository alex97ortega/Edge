using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public Text textTime;
    public Light light;
    float initialTime;

    GameManager gameManager;

    float lightCont = 0.5f;
    bool lightEffect = false;
    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();

        uint totalItems = 0;
        foreach (var x in FindObjectsOfType<Item>())
            totalItems++;
        if(gameManager) gameManager.SetTotalItems(totalItems);

        initialTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
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
        ShowTime(textTime);
    }
    public void ItemGotten()
    {
        lightEffect = true;
        if(gameManager) gameManager.ItemGotten();
    }
    public void LevelPassed()
    {
        if(gameManager) gameManager.LevelPassed(textTime.text);
    }

    private void ShowTime(Text _text)
    {
        string min, seg;
        float _time = Time.time - initialTime; ;
        //min
        if (_time >= 60)        
            min = "0" + (_time / 60).ToString();
        
        else
            min = "00";

        //seg
        if (_time % 60 == 0)
            seg = "00";
        else if (_time % 60 < 10)
            seg = "0" + ((int)_time % 60).ToString();
        else
            seg= ((int)_time % 60).ToString();

        _text.text = min + ":" + seg;
    }
    public void Dead()
    {
        if (gameManager)
            gameManager.Dead();

        // reseteo las posiciones de los posibles bloques 
        // que puedan impedir el avanzar el nivel en la posición actual

        foreach (var b in FindObjectsOfType<SueloQuebradizo>())
            b.RestartBlock();
    }
}
