using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animation _mainMenuAimation;
    [SerializeField] AnimationClip _fadeInAnimation;
    [SerializeField] AnimationClip _fadeOutAnimation;
    

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    [SerializeField] private GameObject MenuButtons;
    [SerializeField] private Button StartButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button QuitButton;

    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private Button ReturnButton;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider EffectsSlider;
    [SerializeField] private Toggle MuteToggle;

    private AudioManager audioManager;



    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.SetMusicVolume(MusicSlider.value);
        audioManager.SetEffectVolume(EffectsSlider.value);
        audioManager.SetMute(MuteToggle.isOn);
        audioManager.UpdateSoundVolumes();


        StartButton.onClick.AddListener(HandleStartClicked);
        OptionsButton.onClick.AddListener(HandleOptionsClicked);
        QuitButton.onClick.AddListener(HandleQuitClicked);
        ReturnButton.onClick.AddListener(HandleReturnClicked);

        MusicSlider.onValueChanged.AddListener(HandleMusicVolumeChanged);
        EffectsSlider.onValueChanged.AddListener(HandleEffectsVolumeChanged);
        MuteToggle.onValueChanged.AddListener(HandleMuteToggle);

        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    public void OnFadeInComplete()
    {
        OnMainMenuFadeComplete.Invoke(false);
        UIManager.Instance.SetDummyCameraActive(true);
    }
    public void OnFadeOutComplete()
    {
        OnMainMenuFadeComplete.Invoke(true);
    }

    public void FadeIn()
    {
        _mainMenuAimation.Stop();
        _mainMenuAimation.clip = _fadeInAnimation;
        _mainMenuAimation.Play();
    }

    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);
        
        _mainMenuAimation.Stop();
        _mainMenuAimation.clip = _fadeOutAnimation;
        _mainMenuAimation.Play();
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {

            EnableMenuButtons(false);
            FadeOut();
        }
        if (previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            EnableMenuButtons(true);
            FadeIn();
        }
    }
    void HandleStartClicked()
    {
        GameManager.Instance.StartGame();
        FadeOut();
    }

    void HandleOptionsClicked()
    {
        ShowOptionsMenu(true);
    }

    void HandleQuitClicked()
    {
        GameManager.Instance.QuitGame();
    }

    void HandleReturnClicked()
    {
        ShowOptionsMenu(false);
    }

    void HandleMusicVolumeChanged(float value)
    {
        audioManager.SetMusicVolume(value);
        audioManager.UpdateSoundVolumes();
    }
    void HandleEffectsVolumeChanged(float value)
    {
        audioManager.SetEffectVolume(value);
        audioManager.UpdateSoundVolumes();
    }

    void HandleMuteToggle(bool isMuted)
    {
        audioManager.SetMute(isMuted);
        if (!isMuted)
        {
            audioManager.SetMusicVolume(MusicSlider.value);
            audioManager.SetEffectVolume(EffectsSlider.value);
        }
        audioManager.UpdateSoundVolumes();
    }


    void EnableMenuButtons(bool isEnabled)
    {
        StartButton.enabled = isEnabled;
        OptionsButton.enabled = isEnabled;
        QuitButton.enabled = isEnabled;
    }

    void ShowOptionsMenu(bool isEnabled)
    {
        MenuButtons.SetActive(!isEnabled);
        OptionsMenu.SetActive(isEnabled);
    }

    
}
