using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerEvents;
using System;

// clase que registra los parámetros del juego que necesitaremos para hacer el análisis
// se va llamando desde el GameManager cuando ocurre un evento notable en el juego 
public class SessionManager : MonoBehaviour {

    #region server_management
    private ServerEventManager eventManager;
    private const string url = "https://intelligence-assessment-tfg.herokuapp.com"; 
    #endregion

    #region session_data
    private string userId;
    private const string gameName = "Edge";
    private DateTime startTime;
    #endregion

    #region custom_events
    public const string GOT_ITEM =       "GOT_ITEM";
    public const string GOT_CHECKPOINT = "GOT_CHECKPOINT";
    #endregion

    public bool enableLogs = true;
    //singletone
    public static SessionManager instance;
    public static SessionManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // initialize event manager using server's events endpoint.
            eventManager = new ServerEventManager(url);
            // Make sure session manager persists between scenes.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // always keep current session manager instead of the one in the scene.
            Destroy(gameObject);
            return;
        }
    }

    // llamadas públicas para llamar desde el GameManager
    public void SetUserId(string id)
    {
        userId = id;
    }

    // starts/ends
    public void StartTutorial()
    {
        LogEvent(ServerEvent.TUTORIAL_START);
    }
    public void EndTutorial()
    {
        LogEvent(ServerEvent.TUTORIAL_END);
    }
    public void StartExperiment()
    {
        LogEvent(ServerEvent.EXPERIMENT_START);
    }
    public void EndExperiment(int points)
    {
        ServerEventParameter[] parameters =
            {new ServerEventParameter(ServerEventParameter.SCORE, points.ToString())};
        LogEvent(ServerEvent.EXPERIMENT_END, parameters);
    }

    public void LevelStart(int levelNumber)
    {
        ServerEventParameter[] parameters =
            {new ServerEventParameter(ServerEventParameter.LEVEL_NUMBER, levelNumber.ToString())};
        LogEvent(ServerEvent.LEVEL_START, parameters);
    }

    public void LevelEnd(int levelNumber)
    {
        ServerEventParameter[] parameters =
            {new ServerEventParameter(ServerEventParameter.LEVEL_NUMBER, levelNumber.ToString())};
        LogEvent(ServerEvent.LEVEL_END, parameters);
    }

    // otras llamadas del gameplay
    public void LogGotItem()
    {
        LogEvent(GOT_ITEM);
    }
    public void LogPlayerDead()
    {
        LogEvent(ServerEvent.PLAYER_DEATH);
    }
    public void LogGotCheckpoint()
    {
        LogEvent(GOT_CHECKPOINT);
    }

    // fill event data with general information to be shared between all events logged from current session.
    private void LogEvent(string eventName, ServerEventParameter[] parameters = null)
    {
        if(enableLogs)
        {
            ServerEvent gameEvent = new ServerEvent(userId, CurrentTimestamp(), gameName, eventName, parameters);
            eventManager.LogEvent(gameEvent);
        }
    }

    private int CurrentTimestamp()
    {
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
}
