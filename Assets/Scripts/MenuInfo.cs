using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInfo : MonoBehaviour {

    public Text levelText, timeText, deadText, itemsText;
    public GameObject menuFinish;
    public GameObject[] terrenos;
    GameManager gameManager;

    float initialTime;

	// Use this for initialization
	void Start () {
        initialTime = Time.time;
        gameManager = FindObjectOfType<GameManager>();
        terrenos[gameManager.GetLevel()-1].SetActive(true);

        ShowValues();
    }
	
    private void ShowValues()
    {
        // LEVEL
        // de momento no va a haber más de 9 niveles
        string lvl = "0" + (gameManager.GetLevel()).ToString();
        levelText.text = "level\n\n " + lvl + "       passed";

        //TIME
        timeText.text = gameManager.GetLevelTime();

        //DEATHS
        deadText.text = gameManager.GetLevelDeaths();

        //ITEMS
        itemsText.text = gameManager.GetLevelItems();
    }
    public void NextLevel()
    {
        gameManager.NextLevel();

        // acaba el tutorial
        if (gameManager.GetLevel() == 4)
            gameManager.EndTutorial();
        // acaba el experimento
        else if (gameManager.GetLevel() == gameManager.numLevels+1) 
        {
            gameManager.EndExperiment();
            menuFinish.SetActive(true);
        }
        else
        {
            float timeInMenu = initialTime - Time.time;
            gameManager.AddMenuTime(timeInMenu);
            gameManager.StartLevel();
        }
    }
    public void ReturnToSessionMenu()
    {
        gameManager.ReturnToSessionMenu();
    }
}
