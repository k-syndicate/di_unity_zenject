using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    public GameObject[] SystemPrefabs;
    public EventGameState OnGameStateChanged;

    List<AsyncOperation> _loadOperations;
    List<GameObject> _instancedSystemPrefabs;
    GameState _currentGameState = GameState.PREGAME;

    string _currentLevelName;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);

        OnGameStateChanged.Invoke(GameState.PREGAME, _currentGameState);
    }

    void Update()
    {
        if (_currentGameState == GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
        }
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        // Clean up level is necessary, go back to main menu
    }

    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch (CurrentGameState)
        {
            case GameState.PREGAME:
                // Initialize any systems that need to be reset
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                //  Unlock player, enemies and input in other systems, update tick if you are managing time
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                // Pause player, enemies etc, Lock other input in other systems
                Time.timeScale = 0.0f;
                break;

            default:
                break;
        }

        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    void HandleMainMenuFadeComplete(bool fadeIn)
    {
        if (!fadeIn)
        {
            UnloadLevel(_currentLevelName);
        }
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }

        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);

        _currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        ao.completed += OnUnloadOperationComplete;
    }

    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public void StartGame()
    {
        LoadLevel("Main");
    }

    public void QuitGame()
    {
        // Clean up application as necessary
        // Maybe save the players game

        Debug.Log("[GameManager] Quit Game.");

        Application.Quit();
    }

    [System.Serializable] public class EventGameState : UnityEvent<GameState, GameState> { }
}
