using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int numNiveles;
    NivelInfo[] infoLevels;
    uint currentLevel;
    bool inTutorial; // we will register Events only if we are NOT playing the tutorial

    struct NivelInfo
    {
        public uint items;
        public uint totalItems;
        public uint deaths;
        public string levelTime;
    }
	// Use this for initialization
	void Start () {
        infoLevels = new NivelInfo[numNiveles];
        currentLevel = 1;
        DontDestroyOnLoad(gameObject);
	}
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Q)) Quit();
    }

    // levels
    public void LevelPassed(string _time)
    {
        infoLevels[currentLevel-1].levelTime = _time;        
        SceneManager.LoadScene("MenuInfo");
    }
    public void NextLevel() { currentLevel++; }
    public uint GetLevel() { return currentLevel; }
    public void StartTutorial()   { currentLevel = 1; inTutorial = true;  SceneManager.LoadScene("Nivel1");  }
    public void StartExperiment() { currentLevel = 4; inTutorial = false; SceneManager.LoadScene("Nivel4");  }

    // items
    public void ItemGotten() {
        infoLevels[currentLevel-1].items++;
    }
    public void SetTotalItems(uint totalItems) { infoLevels[currentLevel - 1].totalItems = totalItems; }

    // player Deaths
    public void Dead() { infoLevels[currentLevel - 1].deaths++; }

    // for level info
    public string GetLevelTime() { return infoLevels[currentLevel - 1].levelTime; }
    public string GetLevelDeaths() { return infoLevels[currentLevel - 1].deaths.ToString(); }
    public string GetLevelItems() {
        string text;
        text = infoLevels[currentLevel - 1].items.ToString() + "/" + infoLevels[currentLevel - 1].totalItems.ToString();
        return text;
    }

    // exit
    public void Quit()
    {
        Application.Quit();
    }
}
