using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : Singleton<GameManager>
{
    // keep track of the game state
    //generate other persistent systems

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
        DEAD
    }

    public GameObject[] SystemPrefabs;
    public Events.EventGameState OnGameStateChanged;
    public Events.EventOutOfLives OnOutOfLives;

    private int playerScore;
    private int lives;
    private int maxLives = 3;
    private List<GameObject> _instancedSystemPrefabs;
    private string _currentLevelName = string.Empty;
    private GameState _currentGameState = GameState.PREGAME;
    private float additionalSpeed;

    List<AsyncOperation> _loadOperations;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();

        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        
    }

    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            UnloadLevel(_currentLevelName);
        }
    }

    public void Update()
    {
        if (_currentGameState == GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        
    }

    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;

            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;
            case GameState.DEAD:
                Time.timeScale = 1.0f;
                break;

            default:
                break;
        }

        OnGameStateChanged.Invoke(_currentGameState, previousGameState);

        //Transition between scenes
    }
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if(_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
        }
        //Debug.Log("Load Complete");
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        //Debug.Log("Unload Complete");
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;

        for(int i = 0; i<SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if(ao == null)
        {
            Debug.LogError("[GameManger] LoadLevel failed to load " + levelName);
            return;
        }
        
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        _currentLevelName = levelName;
    }
    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[GameManger] UnloadLevel failed to unload " + levelName);
            return;
        }

        ao.completed += OnUnloadOperationComplete;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < _instancedSystemPrefabs.Count; ++i)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

    public void StartGame()
    {  
        ResetStats();
        LoadLevel("Main");
        additionalSpeed = 0;
        InvokeRepeating("IncreaseSpeed", 5.0f, 1f);
    }

    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void RestartGame()
    {
        ResetStats();
        UpdateState(GameState.PREGAME);
    }

    public void QuitGame()
    {
        // implement features for quitting like saving
        ResetStats();
        Application.Quit();
    }

    public void AddScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
    }

    public void ResetStats()
    {
        playerScore = 0;
        lives = maxLives;
    }

    public String ScoreString()
    {
        return "Score: " + playerScore;
    }
    public void LossLife()
    {
        if (lives > 1)
        {
            lives--;
        }
        else
        {
            lives--;
            UpdateState(GameState.DEAD);
            OnOutOfLives.Invoke(true);
            CancelInvoke("IncreaseSpeed");
            StartCoroutine(RestartDelayed(3));
        }
    }

    IEnumerator RestartDelayed(float time)
    {
        yield return new WaitForSeconds(time);
        RestartGame();
    }
    public void GainLife()
    {
        if (lives < maxLives)
        {
            lives++;
        }
    }

    public int GetLives()
    {
        return lives;
    }

    public void IncreaseSpeed()
    {
        additionalSpeed += 0.04f;
    }
    public float GetAdditionalSpeed()
    {
        return additionalSpeed;
    }

}
