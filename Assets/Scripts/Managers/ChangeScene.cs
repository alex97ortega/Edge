using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour {

    public LevelManager levelManager;
    private void OnTriggerEnter(Collider other)
    {
        levelManager.LevelPassed();
    }    
}
