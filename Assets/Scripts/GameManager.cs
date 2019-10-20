using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int numNiveles;
    NivelInfo[] infoLevels;
    uint currentLevel;

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
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
    public void LevelPassed(string _time)
    {
        infoLevels[currentLevel-1].levelTime = _time;        
        SceneManager.LoadScene("MenuInfo");
    }
    public void ItemGotten() {
        infoLevels[currentLevel-1].items++;
    }
    public void SetTotalItems(uint totalItems) { infoLevels[currentLevel - 1].totalItems = totalItems; }
    public void NextLevel() { currentLevel++; }
    public void Death() { infoLevels[currentLevel - 1].deaths++; }

    public uint GetLevel() { return currentLevel; }
    // for level info
    public string GetLevelTime() { return infoLevels[currentLevel - 1].levelTime; }
    public string GetLevelDeaths() { return infoLevels[currentLevel - 1].deaths.ToString(); }
    public string GetLevelItems() {
        string text;
        text = infoLevels[currentLevel - 1].items.ToString() + "/" + infoLevels[currentLevel - 1].totalItems.ToString();
        return text;
    }
}
