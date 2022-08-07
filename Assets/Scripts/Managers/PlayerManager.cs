/* Author : Mehmet Bedirhan U?ak*/
using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;

public class PlayerManager : Singleton<PlayerManager>
{
    private GameManager _gameManager;
    private ComponentManager _componentManager;
    private HitDetection _hitDetection;
    private MotoRigController _rigController;

    [BoxGroup("Player Gameobject")]
    public GameObject Player1;
    public GameObject Player2;
    
    public float PlayerSpeed;
    [BoxGroup("Player Movement Options")]
    public float DefaultPlayerSpeed = 6;
    [BoxGroup("Player Movement Options")]
    public float PlayerSideSpeed = 50;
    private float _backupSideSpeed;
    [BoxGroup("Player Dead Count Options")]
    public int PlayerMaxDeadCount = 3;
    [BoxGroup("Player Dead Count Options")]
    [OnValueChanged("OnValueChangedCallback")]
    public int PlayerCurrentDeadCount = 0;
    [BoxGroup("Player Animation Controller")]
    public bool HavePlayerAnimations;
    [BoxGroup("Player Animation Controller")]
    public Animator MalePlayerAnimationController;
    public Animator FemalePlayerAnimationController;
    [BoxGroup("Player Animation Controller")]
    public bool PlayerWalkingMode;
    [BoxGroup("Player Animation Controller")]
    public bool PlayerWinDance;
    [BoxGroup("Player Animation Controller")]
    public bool PlayerWinNoDance;
    public int bumpCount = 0;
    public GameObject cmCameraLook;
    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _componentManager = ComponentManager.Instance;
        _hitDetection = HitDetection.Instance;
        _backupSideSpeed = PlayerSideSpeed;
    }

    #region Player Start And Stop Options
    public void StopPlayer(bool isLose,bool isWinDance,bool isWin, bool isCaught, bool hitDog)
    {
        PlayerSpeed = 0;
        PlayerSideSpeed = 0;

        if (HavePlayerAnimations)
        {
            MalePlayerAnimationController.SetBool("isBackState", false);
            FemalePlayerAnimationController.SetBool("isBackState", false);

            if (isWinDance)
            {
            MalePlayerAnimationController.SetBool("isWinDance", true);
            FemalePlayerAnimationController.SetBool("isWinDance", true);
            }
                
            if (isWin)
            {
            //MalePlayerAnimationController.SetBool("isWin", true);
            //FemalePlayerAnimationController.SetBool("isWin", true);
            MalePlayerAnimationController.SetBool("isWinDance", true);
            FemalePlayerAnimationController.SetBool("isWinDance", true);
            }

            if (isCaught)
            {
                MalePlayerAnimationController.SetBool("hitPolice", true);
                FemalePlayerAnimationController.SetBool("hitPolice", true);

                MalePlayerAnimationController.SetBool("isRuning", false);
                FemalePlayerAnimationController.SetBool("isRuning", false);
            }

            if (hitDog)
            {
                MalePlayerAnimationController.SetBool("hitDog", true);
                FemalePlayerAnimationController.SetBool("hitDog", true);

                MalePlayerAnimationController.SetBool("isRuning", false);
                FemalePlayerAnimationController.SetBool("isRuning", false);
            }
                
            if (isLose)
            {
            MalePlayerAnimationController.SetBool("isLose", true);
            FemalePlayerAnimationController.SetBool("isLose", true);
            }
                
        }
    }

    /// <summary>
    /// IdleStatePlayer();
    /// For Joypad controls only.
    /// </summary>
    public void IdleStatePlayer()
    {
        if (HavePlayerAnimations)
        {
           MalePlayerAnimationController.SetBool("isWalking", false);
           FemalePlayerAnimationController.SetBool("isRuning", false);

           MalePlayerAnimationController.SetBool("isWalking", false);
           FemalePlayerAnimationController.SetBool("isRuning", false);
        }
    }

    public void StartPlayer(bool isRuning)
    {
        
        

        PlayerSpeed = DefaultPlayerSpeed;
        PlayerSideSpeed = _backupSideSpeed;
        if (HavePlayerAnimations)
        {
            MalePlayerAnimationController.SetBool("isBackState", false);
            FemalePlayerAnimationController.SetBool("isBackState", false);

            if (!isRuning)
            {
            MalePlayerAnimationController.SetBool("isWalking", true);
            FemalePlayerAnimationController.SetBool("isWalking", true);
            }
            else
            {
            MalePlayerAnimationController.SetBool("isRuning", true);
            FemalePlayerAnimationController.SetBool("isRuning", true);
            }
                
        }
       
    }

    public async void RestartPlayer()
    {
        if (_hitDetection.motorcyclePlayer.activeInHierarchy)
        {
            _hitDetection.motorcyclePlayer.SetActive(false);
        }
        
        resetPlayerAnimationStates();

        Player1.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Player2.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().enabled = false;

        Player1.transform.position = new Vector3(-0.4f, 1, 0);
        Player2.transform.position = new Vector3(0.4f, 1, 0);

        

        Player1.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
        Player2.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);

        PlayerCurrentDeadCount = 0;

        await Task.Delay(100);

        Player1.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().enabled = true;
        Player2.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().enabled = true;
        bumpCount = 0;
        _componentManager.P3DPainter.SetActive(false);
        _hitDetection.alreadyBumped = false;
        _hitDetection.withinSprayArea = false;
        _hitDetection.motorcyclePlayer.SetActive(false);
        _gameManager.UpdateGameState(GameState.StartGame);
        
    }


    private void resetPlayerAnimationStates()
    {
        if (HavePlayerAnimations)
        {
            MalePlayerAnimationController.SetBool("isWalking", false);
            MalePlayerAnimationController.SetBool("isRuning", false);
            MalePlayerAnimationController.SetBool("isLose", false);
            MalePlayerAnimationController.SetBool("isWinDance", false);
            MalePlayerAnimationController.SetBool("isWin", false);
            MalePlayerAnimationController.SetBool("isBumped", false);
            MalePlayerAnimationController.SetBool("isBombing", false);
            MalePlayerAnimationController.SetBool("hitPolice", false);
            MalePlayerAnimationController.SetBool("hitDog", false);
            MalePlayerAnimationController.SetBool("isFinished", false);
            MalePlayerAnimationController.SetBool("isBackState", true);


            FemalePlayerAnimationController.SetBool("isWalking", false);
            FemalePlayerAnimationController.SetBool("isRuning", false);
            FemalePlayerAnimationController.SetBool("isLose", false);
            FemalePlayerAnimationController.SetBool("isWinDance", false);
            FemalePlayerAnimationController.SetBool("isWin", false);
            FemalePlayerAnimationController.SetBool("isBumped", false);
            FemalePlayerAnimationController.SetBool("isBombing", false);
            FemalePlayerAnimationController.SetBool("hitPolice", false);
            FemalePlayerAnimationController.SetBool("hitDog", false);
            FemalePlayerAnimationController.SetBool("isFinished", false);
            FemalePlayerAnimationController.SetBool("isBackState", true);
        }
  
    }
    #endregion

    #region Player Health Options
    public void AddDamage(int amount)
    {
        PlayerCurrentDeadCount += amount;
        OnValueChangedCallback();
    }

    public void AddPoliceDamage(int amount)
    {
        PlayerCurrentDeadCount += amount;
        PoliceDamage();
    }

    private void PoliceDamage()
    {
        if (PlayerCurrentDeadCount >= PlayerMaxDeadCount)
        {
            
            _gameManager.UpdateGameState(GameState.Caught);
        }
    }

    public void AddDogDamage(int amount)
    {
        PlayerCurrentDeadCount += amount;
        DogDamage();
    }

    private void DogDamage()
    {
        if (PlayerCurrentDeadCount >= PlayerMaxDeadCount)
        {
            _gameManager.UpdateGameState(GameState.HitDog);
        }
    }
    #endregion

    private void OnValueChangedCallback()
    {
        if(PlayerCurrentDeadCount >= PlayerMaxDeadCount)
        {
            _gameManager.UpdateGameState(GameState.LoseGame);
        }
    }

}
