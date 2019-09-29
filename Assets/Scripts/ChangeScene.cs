using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour {

    GameManager gm;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        gm.LevelPassed();
    }    
}
