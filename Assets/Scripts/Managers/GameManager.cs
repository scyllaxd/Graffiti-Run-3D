/* Author : Mehmet Bedirhan U?ak*/
using System;
using UnityEngine;
using NaughtyAttributes;


public class GameManager : Singleton<GameManager>
{
    [OnValueChanged("OnValueChangedCallback")]
    public GameState State;
    [HideInInspector]
    public TimeState StateTime;
    private UIManager _uiManager;
    private PlayerManager _playerManager;
    private CollectManager _collectManager;
    private ComponentManager _componentManager;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        _uiManager = UIManager.Instance;
        _playerManager = PlayerManager.Instance;
        _collectManager = CollectManager.Instance;
        _componentManager = ComponentManager.Instance;

    }

    private void Start()
    {
        UpdateGameState(GameState.WaitGame);
    }

    #region Game State Options
    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.WinGame:
                handleWinGame();
            break;
            case GameState.LoseGame:
                handleLoseGame();
            break;
            case GameState.Caught:
                handleCaught();
            break;
            case GameState.HitDog:
                handleDog();
            break;
            case GameState.StartGame:
                handleStartGame();
            break;
            case GameState.WaitGame:
                handleWaitGame();
            break;
            case GameState.RestartGame:
                handleRestartGame();
            break;
        }
    }

    public void UpdateGameTimeState(TimeState newState,float amount)
    {
        StateTime = newState;

        switch (newState)
        {
            case TimeState.AddTime:
                handleAddTime(amount);
                break;
            case TimeState.DecreaseTime:
                handleDecreaseTime(amount);
            break;
        }
    }

    public void UpdateGameMaxTimeState(TimeState newState, float amount)
    {
        StateTime = newState;

        switch (newState)
        {
            case TimeState.HandleSetMaxTime:
                handleSetMaxTime(amount);
            break;
        }
    }



    private void handleWaitGame()
    {
        _uiManager.UpdatePanelState(PanelCode.StartPanel, true);
        _playerManager.StopPlayer(false,false,false, false, false);
        
    }
    private void handleStartGame()
    {
        _uiManager.UpdatePanelState(PanelCode.GamePanel, true);
        _componentManager.TimerSliderReset();
        
        if (!_componentManager.JoypadMode)
            _playerManager.StartPlayer(_playerManager.PlayerWalkingMode);
    }
    private void handleWinGame()
    {
        _uiManager.UpdatePanelState(PanelCode.WinPanel, true);
        _componentManager.TimerSliderReset();
        _playerManager.StopPlayer(false,_playerManager.PlayerWinDance,_playerManager.PlayerWinNoDance, false, false);
        
    }
    private void handleLoseGame()
    {
        _uiManager.UpdatePanelState(PanelCode.LosePanel, true);
        _componentManager.TimerSliderReset();
        _playerManager.StopPlayer(true,false,false,false, false);

    }

    private void handleCaught()
    {
        _uiManager.UpdatePanelState(PanelCode.LosePanel, true);
        _componentManager.TimerSliderReset();
        _playerManager.StopPlayer(false, false, false, true, false);
    }

    private void handleDog()
    {
        _uiManager.UpdatePanelState(PanelCode.LosePanel, true);
        _componentManager.TimerSliderReset();
        _playerManager.StopPlayer(false, false, false, false, true);
    }
    private void handleRestartGame()
    {
        _uiManager.UpdatePanelState(PanelCode.GamePanel, true);
        _playerManager.RestartPlayer();
        _collectManager.ActiveCollectedObject();
        _collectManager.ActiveObstacleObject();
        _componentManager.TimerSliderReset();
    }
    private void handleAddTime(float amount)
    {
        _componentManager.TimerSliderComponent.value += amount;
    }
    private void handleDecreaseTime(float amount)
    {
        _componentManager.TimerSliderComponent.value -= amount;
    }

    private void handleSetMaxTime(float amount)
    {
        _componentManager.TimerSliderTime = amount;
    }
    private void OnValueChangedCallback()
    {
        UpdateGameState(State);
    }
    #endregion

}


public enum GameState
{
    WinGame,
    LoseGame,
    StartGame,
    WaitGame,
    Caught,
    HitDog,
    RestartGame 
}

public enum TimeState
{
    AddTime,
    DecreaseTime,
    HandleSetMaxTime
}