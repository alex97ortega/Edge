using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int numNiveles;
    int[] items;
    uint currentLevel;
    uint deaths;

	// Use this for initialization
	void Start () {
        items = new int[numNiveles];
        currentLevel = 1;
        deaths = 0;
        DontDestroyOnLoad(gameObject);
	}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
    public void LevelPassed()
    {
        //provisional hasta que haya menús
        currentLevel++;
        deaths = 0; // habrá que guardarlo
        string escena = "Nivel" + currentLevel.ToString();
        SceneManager.LoadScene(escena);
    }
    public void ItemGotten() {
        items[currentLevel-1]++;
    }
    public void Death() { deaths++; }
}
