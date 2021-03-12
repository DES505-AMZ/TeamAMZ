using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void EventGameState(GameManager.GameState curState, GameManager.GameState preState);

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    public event EventGameState OnGameStateChange;

    GameState currentGameState = GameState.PREGAME;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
        private set { currentGameState = value; }
    }

    string currentSceneName = string.Empty;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        if(currentScene != currentSceneName)
        {
            currentSceneName = currentScene;
            UpdateState(GameState.RUNNING);
        }
    }

    void UpdateState(GameState state)
    {
        GameState preGameState = currentGameState;
        currentGameState = state;
        switch(currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.None;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                break;
            default:
                break;
        }

        if(OnGameStateChange != null)
            OnGameStateChange(currentGameState, preGameState);
    }

    public void PauseGame()
    {
        if (currentGameState == GameState.RUNNING)
            UpdateState(GameState.PAUSED);
    }

    public void ResumeGame()
    {
        if (currentGameState == GameState.PAUSED)
            UpdateState(GameState.RUNNING);
    }

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        Debug.Log(currentSceneName + " load Complete");
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log(currentSceneName + " unload Complete");
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName);
        if(ao == null)
        {
            Debug.LogError("GameManager: Unable to load level" + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        currentSceneName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("GameManager: Unable to load level" + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }

    void StartGame()
    {
        LoadLevel(GameConstants.k_SceneNameMainMenu);
    }

    void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
