using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] MainMenu _mainMenu;
    [SerializeField] PauseMenu _pauseMenu;
    [SerializeField] Camera _dummyCamera;
    [SerializeField] HUD _hud;
    [SerializeField] TextMeshProUGUI _scoreText;
    GameManager.GameState _currentState;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        _mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        OnMainMenuFadeComplete.Invoke(fadeOut);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
        _currentState = currentState;
    }

    public void Update()
    {
        if(_currentState != GameManager.GameState.PREGAME)
        {
            _hud.gameObject.SetActive(true);
            _scoreText.text = GameManager.Instance.ScoreString();
            return;
        }

        else
        {
            _hud.gameObject.SetActive(false);
        }
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }
}
