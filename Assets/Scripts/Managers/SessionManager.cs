using UnityEngine;
using System;
using System.Runtime.InteropServices;

// clase que registra los parámetros del juego que necesitaremos para hacer el análisis
// se va llamando desde el GameManager cuando ocurre un evento notable en el juego 
public class SessionManager : MonoBehaviour {

    #region server_management
    //private const string url = "https://intelligence-assessment-tfg.herokuapp.com"; 
    #endregion

    #region session_data
    private string userId;
    private const string gameName = "Edge";
    private DateTime startTime;
    private int orderInSequence = 0;
    #endregion

    #region custom_events
    public const string GOT_ITEM =       "GOT_ITEM";
    public const string GOT_CHECKPOINT = "GOT_CHECKPOINT";
    public const string NUM_MOVEMENTS  = "NUM_MOVEMENTS";
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

    [DllImport("__Internal")]
    private static extern void LogGameEvent(string eventJSON);

    [DllImport("__Internal")]
    private static extern void GameOver();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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


    // starts/ends
    public void StartTutorial()
    {
        LogEvent(WebEvent.TUTORIAL_START);
    }
    public void EndTutorial()
    {
        LogEvent(WebEvent.TUTORIAL_END);
    }
    public void StartExperiment()
    {
        LogEvent(WebEvent.EXPERIMENT_START);
    }
    public void EndExperiment(int points)
    {
        WebEventParameter[] parameters =
            {new WebEventParameter(WebEventParameter.SCORE, points.ToString())};
        LogEvent(WebEvent.EXPERIMENT_END, parameters);

        // notify browser about game end
        if (enableLogs)
            GameOver();
    }

    public void LevelStart(int levelNumber)
    {
        WebEventParameter[] parameters =
            {new WebEventParameter(WebEventParameter.LEVEL_NUMBER, levelNumber.ToString())};
        LogEvent(WebEvent.LEVEL_START, parameters);
    }

    public void LevelEnd(int levelNumber, int movements)
    {
        WebEventParameter[] parameters =
            {new WebEventParameter(WebEventParameter.LEVEL_NUMBER, levelNumber.ToString()),
             new WebEventParameter(NUM_MOVEMENTS, movements.ToString())};
        LogEvent(WebEvent.LEVEL_END, parameters);
    }

    // otras llamadas del gameplay
    public void LogGotItem()
    {
        LogEvent(GOT_ITEM);
    }
    public void LogPlayerDead()
    {
        LogEvent(WebEvent.PLAYER_DEATH);
    }
    public void LogGotCheckpoint()
    {
        LogEvent(GOT_CHECKPOINT);
    }

    // fill event data with general information to be shared between all events logged from current session.
    private void LogEvent(string eventName, WebEventParameter[] parameters = null)
    {
        if(enableLogs)
        {
            WebEvent webEvent = new WebEvent
            {
                name = eventName,
                parameters = parameters,
                timestamp = CurrentTimestamp(),
                gameName = gameName,
                orderInSequence = orderInSequence
            };
            orderInSequence++;
            LogGameEvent(JsonUtility.ToJson(webEvent));
        }
    }

    private int CurrentTimestamp()
    {
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
    
}
