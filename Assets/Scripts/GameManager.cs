using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int numNiveles;
    int[] items;
    int currentLevel;

	// Use this for initialization
	void Start () {
        items = new int[numNiveles];
        currentLevel = 1;
        DontDestroyOnLoad(gameObject);
	}
    
    public void LevelPassed()
    {
        //provisional hasta que haya menús
        currentLevel++;
        string escena = "Nivel" + currentLevel.ToString();
        SceneManager.LoadScene(escena);
    }
    public void ResetNivel()
    {
        string escena = "Nivel" + currentLevel.ToString();
        SceneManager.LoadScene(escena);
    }
    public void ItemGotten() {
        items[currentLevel-1]++;
    }
}
