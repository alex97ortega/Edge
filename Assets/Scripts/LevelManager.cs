using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public Light light;

    GameManager gameManager;
    float lightCont = 0.5f;
    bool lightEffect = false;
    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (lightEffect)
        {
            lightCont += 0.05f;
            light.color = new Color(lightCont, lightCont, 1);
            if (lightCont >= 1)
            {
                light.color = new Color(1, 1, 1);
                lightEffect = false;
                lightCont = 0.5f;
            }
        }
    }
    public void ItemGotten()
    {
        lightEffect = true;
        if(gameManager) gameManager.ItemGotten();
    }
}
