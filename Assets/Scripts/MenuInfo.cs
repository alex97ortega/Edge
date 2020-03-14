using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        gameManager.NextLevel();

        if (gameManager.GetLevel() == 4)
            SceneManager.LoadScene("MainMenu"); // no permitimos pasar de tutorial a experimento sin pasar por MainMenu
        else if(gameManager.GetLevel() == gameManager.numLevels+1) // informamos si ha llegado al tope de niveles
        {
            menuFinish.SetActive(true);
        }
        else
        {
            float timeInMenu = initialTime - Time.time;
            gameManager.AddMenuTime(timeInMenu);
            string escena = "Nivel" + (gameManager.GetLevel()).ToString();
            SceneManager.LoadScene(escena);
        }
    }
    public void ReturnToSessionMenu()
    {
        gameManager.ReturnToSessionMenu();
    }
}
