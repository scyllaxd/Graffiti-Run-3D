/* Author : Mehmet Bedirhan U?ak*/
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class ComponentManager : Singleton<ComponentManager>
{

    private UIManager _uiManager;
    private LevelManager _levelManager;
    private GameManager _gameManager;
    private CollectManager _collectManager;
    private PlayerManager _playerManager;
    private HitDetection _hitDetection;

    public GameObject P3DPainter;

    [BoxGroup("Joystick Mode")]
    public bool JoypadMode;
    [BoxGroup("Joystick Mode")]
    public GameObject Floating_Joystick;
    
    [BoxGroup("Joystick Mode")]
    public PlayerMovementController playerMovementController;
    
    [BoxGroup("Joystick Mode")]
    public PlayerMovementJoypadController playerMovementJoypadController;


    [BoxGroup("Buton")]
    public Button PlayButton;
    [BoxGroup("Buton")]
    public Button RetryButton;
    [BoxGroup("Buton")]
    public Button WinButton;
    
    [BoxGroup("Butons Text")]
    public TextMeshProUGUI PlayButtonText;
    [BoxGroup("Butons Text")]
    public TextMeshProUGUI RetryButtonText;
    [BoxGroup("Butons Text")]
    public TextMeshProUGUI WinButtonText;

    [BoxGroup("Game Panel Texts")]
    public TextMeshProUGUI LevelNumberTextTimerSlider;
    [BoxGroup("Game Panel Texts")]
    public TextMeshProUGUI LevelNumberTextDistanceSlider;
    [BoxGroup("Game Panel Texts")]
    public TextMeshProUGUI CoinNumberText;
    [BoxGroup("Game Panel Texts")]
    public TextMeshProUGUI CrystalNumberText;

    [BoxGroup("Level Number Text No Slider")]
    public GameObject LevelNumberTextNoSlider;


    [BoxGroup("Game Panel Component Options")]
    public GameObject CrystalHolder;
    [BoxGroup("Game Panel Component Options")]
    public bool isCrystalHolder;

    [BoxGroup("Distance Slider Options")]
    [HideIf("isTimerSlider")]
    public bool isDistanceSlider;
    [BoxGroup("Distance Slider Options")]
    [HideIf("isTimerSlider")]
    public GameObject DistanceSlider;
    [BoxGroup("Distance Slider Options")]
    [HideIf("isTimerSlider")]
    public Slider DistanceSliderComponent;



    [BoxGroup("Timer Slider Options")]
    [HideIf("isDistanceSlider")]
    public bool isTimerSlider;
    [BoxGroup("Timer Slider Options")]
    [HideIf("isDistanceSlider")]
    public GameObject TimerSlider;
    [BoxGroup("Timer Slider Options")]
    [HideIf("isDistanceSlider")]
    public Slider TimerSliderComponent;
    private float TimerSliderAmount = 1f;
    [BoxGroup("Timer Slider Options")]
    [HideIf("isDistanceSlider")]
    public float TimerSliderTime;
    [BoxGroup("Timer Slider Options")]
    [HideIf("isDistanceSlider")]
    public bool isIncrase;

    [BoxGroup("Remaning Time Debug")]
    public TextMeshProUGUI RemaningTimeText;

    private float dist;
    private GameObject FinishLine;
    public bool painterActive = true;

    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        _uiManager = UIManager.Instance;
        _gameManager = GameManager.Instance;
        _collectManager = CollectManager.Instance;
        _levelManager = LevelManager.Instance;
        _playerManager = PlayerManager.Instance;
        _hitDetection = HitDetection.Instance;
    }

    private void Start()
    {
        PlayButton.onClick.AddListener(() => HandlePlayButton());
        WinButton.onClick.AddListener(() => HandleNextButton());
        RetryButton.onClick.AddListener(() => HandleRetryButton());
        LevelNumberTextNoSlider.SetActive(true);
        //JoypadModeEnabled();
        if (isCrystalHolder)
        {
            CrystalHolder.SetActive(true);
        }

        //P3DPainter.SetActive(false);

    }

    private void Update()
    {
        CoinNumberText.text = _collectManager.CollectedCoin.ToString();
        CrystalNumberText.text = _collectManager.CollectedCrystal.ToString();
        LevelNumberTextNoSlider.GetComponent<TextMeshProUGUI>().text = "LEVEL " + _levelManager.DisplayLevelNumer.ToString();
        LevelNumberTextTimerSlider.text = "LEVEL " + _levelManager.DisplayLevelNumer.ToString();
        LevelNumberTextDistanceSlider.text = "LEVEL " + _levelManager.DisplayLevelNumer.ToString();

        if (isDistanceSlider)
        {
            FinishLine = GameObject.FindGameObjectWithTag("Finish");
            dist = _playerManager.Player1.transform.position.z - (FinishLine.transform.position.z - _playerManager.Player1.transform.position.z) * -0.1f;
            float value = dist / FinishLine.transform.position.z;
            DistanceSliderComponent.value = value;
        }

        if (isTimerSlider && _gameManager.State == GameState.StartGame)
        {
            if (isIncrase)
            {
                TimerSliderComponent.value += TimerSliderAmount * Time.deltaTime;
                if(TimerSliderComponent.value > TimerSliderComponent.maxValue)
                    _gameManager.UpdateGameState(GameState.LoseGame);

            }
            else
            {
                //TimerSliderComponent.value -= TimerSliderAmount * Time.deltaTime;

                if (TimerSliderComponent.value <= 0 || _hitDetection.alreadyBumped==true || _hitDetection.sprayAvailable == false)
                {
                    //_gameManager.UpdateGameState(GameState.LoseGame);
                    if(painterActive)
                    {
                        P3DPainter.gameObject.SetActive(false);
                        painterActive = false;
                    }
                    
                }
                else
                {
                    if(!painterActive)
                    {
                        P3DPainter.gameObject.SetActive(true);
                        painterActive = true;
                    }
                    
                }
                    
            }
        }
    }

    #region UI Button Options
    private void HandleNextButton()
    {
        _levelManager.NextLevel();
        _gameManager.UpdateGameState(GameState.RestartGame);
        handleSetMaxTime(TimerSliderTime);
        HandleDistanceSlider();
    }

    private void HandleRetryButton()
    {
        //virtualCamera.gameObject.SetActive(false);
        //virtualCamera.Follow = null;
        //virtualCamera.LookAt = null;
        
        _hitDetection.MalePlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        _hitDetection.FemalePlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        GameObject.Find("Hair").GetComponent<SkinnedMeshRenderer>().enabled = true;
        GameObject.Find("Kirpik").GetComponent<SkinnedMeshRenderer>().enabled = true;
        GameObject.Find("Left_Shoes").GetComponent<SkinnedMeshRenderer>().enabled = true;
        

        _levelManager.RestartLevel();
        _gameManager.UpdateGameState(GameState.RestartGame);
        handleSetMaxTime(TimerSliderTime);
        HandleDistanceSlider();
        
    }

    private void HandlePlayButton()
    {
        _gameManager.UpdateGameState(GameState.StartGame);
        handleSetMaxTime(TimerSliderTime);
        HandleDistanceSlider();
    }
    #endregion

    #region Distane Slider Component

    private void HandleDistanceSlider()
    {
        if (isDistanceSlider)
        {
            DistanceSlider.SetActive(true);
            LevelNumberTextNoSlider.SetActive(false);
            return;
        }
    }

    #endregion

    #region Timer Slider Component
    public void handleSetMaxTime(float amount)
    {
        if (isTimerSlider)
        {
            TimerSlider.SetActive(true);
            LevelNumberTextNoSlider.SetActive(false);
            //TimerSliderTime = amount;
            TimerSliderTime = 0;
            if (isIncrase)
            {
                TimerSliderComponent.maxValue = amount;
                TimerSliderComponent.value = 0;
            }
            else
            {
                //TimerSliderComponent.maxValue = amount;
                //TimerSliderComponent.value = amount;

                TimerSliderComponent.maxValue = amount;
                TimerSliderComponent.value = 0;
            }

            return;
        }

        DistanceSlider.SetActive(false);
    }


    public void TimerSliderReset()
    {
        TimerSliderComponent.value = 0;
    }
    #endregion

    #region Joypad Controls
    /*public void JoypadModeEnabled()
    {
        if (JoypadMode)
        {
            Floating_Joystick.SetActive(true);
            playerMovementController.enabled = false;
            playerMovementJoypadController.enabled = true;
        }
        else
        {
           Floating_Joystick.SetActive(false);
           playerMovementController.enabled = true;
           playerMovementJoypadController.enabled = false;
        }
    }*/
    #endregion

}
