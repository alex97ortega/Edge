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
        gameManager.CalculatePoints(true);

        ShowValues();
    }
	
    private void ShowValues()
    {
        // LEVEL
        // de momento no va a haber más de 9 niveles
        string lvl = "0" + (gameManager.GetLevel()).ToString();
        levelText.text = "level\n\n " + lvl + "       passed";

        //TIME
        timeText.text = ConvertTimeToMinSeg(gameManager.GetLevelTime());

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
            gameManager.ReturnToMainMenu();
        // acaba el experimento
        else if (gameManager.GetLevel() == gameManager.numLevels+1) 
        {
            menuFinish.SetActive(true);
        }
        else
        {
            float timeInMenu = initialTime - Time.time;
            gameManager.AddMenuTime(timeInMenu);
            gameManager.StartLevel();
        }
    }

    private string ConvertTimeToMinSeg(float _time)
    {
        string min, seg;
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

        string result = min + ":" + seg;
        return result;
    }
    public void ReturnToMainMenu()
    {
        gameManager.ReturnToMainMenu();
    }
}
