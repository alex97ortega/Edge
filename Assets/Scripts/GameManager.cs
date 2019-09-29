using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int numNiveles;
    Light light;
    int[] items;
    int currentLevel;
    float lightCont = 0.5f;
    bool lightEffect = false;

	// Use this for initialization
	void Start () {
        light = FindObjectOfType<Light>();
        items = new int[numNiveles];
        currentLevel = 1;
        DontDestroyOnLoad(gameObject);
	}

    private void Update()
    {
        if (lightEffect)
        {
            lightCont += 0.05f;
            light.color = new Color(lightCont, lightCont,1);
            if (lightCont >= 1)
            {
                light.color = new Color(1, 1, 1);
                lightEffect = false;
                lightCont = 0.5f;
            }
        }
    }
    public void LevelPassed()
    {
        //provisional hasta que haya menús
        currentLevel++;
        string escena = "Nivel" + currentLevel.ToString();
        SceneManager.LoadScene(escena);
        light = FindObjectOfType<Light>();
    }
    public void ItemGotten() {
        items[currentLevel-1]++;
        lightEffect = true;
    }
}
