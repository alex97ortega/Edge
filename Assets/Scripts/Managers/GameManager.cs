using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int numLevels;
    public int experimentTime;// in secs
    NivelInfo[] infoLevels;
    uint currentLevel;
    bool inTutorial = false;

    float initialTimeExperiment; // we register the time when the player starts the experiment
    float timeInMenus; // we dont count experiment time while the player is in a menu

    struct NivelInfo
    {
        public uint items;
        public uint totalItems;
        public uint deaths;
        public string levelTime;
    }
    SessionManager sessionManager;
    
    // Use this for initialization
    void Start () {
        sessionManager = FindObjectOfType<SessionManager>();
        infoLevels = new NivelInfo[numLevels];
        currentLevel = 1;
        initialTimeExperiment = 0;
        timeInMenus = 0;
        DontDestroyOnLoad(gameObject);
	}
    

    // levels
    public void LevelPassed(string _time)
    {
        sessionManager.LevelEnd((int)currentLevel);
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
        sessionManager.StartTutorial();
        StartLevel();
    }

    public void StartExperiment()
    {
        currentLevel = 4;
        inTutorial = false;
        sessionManager.StartExperiment();
        StartLevel();
    }
    public void StartLevel()
    {
        sessionManager.LevelStart((int)currentLevel);
        string escena = "Nivel" + currentLevel.ToString();
        SceneManager.LoadScene(escena);
    }

    public void StartSession(string id) {
        sessionManager.SetUserId(id);
        ReturnToMainMenu();
    }
    public void ReturnToMainMenu()
    {
        if (inTutorial)
            sessionManager.EndTutorial();
        else if(initialTimeExperiment != 0)
        {
            sessionManager.EndExperiment();
            initialTimeExperiment = 0;
            timeInMenus = 0;
        }
        SceneManager.LoadScene("MainMenu");
    }
    public void ReturnToSessionMenu() {

        SceneManager.LoadScene("StartSession");

        // Destruimos este GameManager. Con esto nos quitamos 2 problemas:
        // 1- Tener que resetear todos los valores.
        // 2- Tener conflicto por crearse otro GameManager en la escena StartSession, 
        //    que es a la que vamos.
        Destroy(gameObject);
    }
    

    // items
    public void ItemGotten() {
        sessionManager.LogGotItem();
        infoLevels[currentLevel-1].items++;
    }
    public void SetTotalItems(uint totalItems) { infoLevels[currentLevel - 1].totalItems = totalItems; }

    // player Deaths
    public void Dead() {
        sessionManager.LogPlayerDead();
        infoLevels[currentLevel - 1].deaths++;
    }
    // checkpoint
    public void GotCheckpoint()
    {
        sessionManager.LogGotCheckpoint();
    }

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
    public int GetExperimentTime() { return experimentTime; }
    public float GetMenuTime() { return timeInMenus; }
    public void AddMenuTime(float timeToAdd) { if(!inTutorial) timeInMenus += timeToAdd; }

    // exit
    public void Quit()
    {
        Application.Quit();
    }
}
