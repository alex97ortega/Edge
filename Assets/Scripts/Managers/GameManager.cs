using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int numNiveles;
    public int minutosExperimento;
    NivelInfo[] infoLevels;
    uint currentLevel;
    bool inTutorial = false; // we will register Events only if we are NOT playing the tutorial

    float initialTimeExperiment; // we register the time when the player starts the experiment
    uint experimentTime; // in secs
    float timeInMenus; // we dont count experiment time while the player is in a menu

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
        experimentTime = (uint)minutosExperimento * 60;//in secs, 12 min time experiment
        initialTimeExperiment = 0;
        timeInMenus = 0;
        DontDestroyOnLoad(gameObject);
	}

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q)) Quit();
    }

    // levels
    public void LevelPassed(string _time)
    {
        infoLevels[currentLevel-1].levelTime = _time;        
        SceneManager.LoadScene("MenuInfo");
    }
    public void NextLevel() { currentLevel++; }
    public uint GetLevel() { return currentLevel; }

    // starts
    public void StartTutorial()
    {
        currentLevel = 1;
        inTutorial = true;
        SceneManager.LoadScene("Nivel1");
    }

    public void StartExperiment()
    {
        currentLevel = 4;
        inTutorial = false;
        SceneManager.LoadScene("Nivel4");
    }

    public void StartSession() { SceneManager.LoadScene("MainMenu"); }

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
    public bool HasPlayedTutorial() { return inTutorial; }

    // related to experiment times
    public void SetInitialTimeExperiment()
    {
        if (!inTutorial && initialTimeExperiment == 0)
            initialTimeExperiment = Time.time;
    }
    public float GetInitialTimeExperiment() { return initialTimeExperiment; }
    public uint GetExperimentTime() { return experimentTime; }
    public float GetMenuTime() { return timeInMenus; }
    public void AddMenuTime(float timeToAdd) { if(!inTutorial) timeInMenus += timeToAdd; }

    // exit
    public void Quit()
    {
        Application.Quit();
    }
}
