﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuInfo : MonoBehaviour {

    public Text levelText, timeText, deadText, itemsText;
    public GameObject[] terrenos;
    GameManager gameManager;
	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        terrenos[gameManager.GetLevel()-1].SetActive(true);

        ShowValues();
    }
	
    private void ShowValues()
    {
        // LEVEL
        // de momento no va a haber más de 9 niveles
        string lvl = "0" + (gameManager.GetLevel()).ToString();
        levelText.text = "level\n\n " + lvl + "                   passed";

        //TIME
        timeText.text = gameManager.GetLevelTime();

        //DEATHS
        deadText.text = gameManager.GetLevelDeaths();

        //ITEMS
        itemsText.text = gameManager.GetLevelItems();
    }
    public void NextLevel()
    {
        if (gameManager.GetLevel() + 1 > gameManager.numNiveles) return;

        gameManager.NextLevel();
        string escena = "Nivel" + (gameManager.GetLevel()).ToString();
        SceneManager.LoadScene(escena);
    }
}
