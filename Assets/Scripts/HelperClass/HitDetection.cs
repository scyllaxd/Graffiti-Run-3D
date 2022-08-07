/* Author : Mehmet Bedirhan U?ak*/
using UnityEngine;
using System.Threading.Tasks;
using NaughtyAttributes;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UI;

public class HitDetection : Singleton<HitDetection>
{
    private CollectManager _collectManager;
    private ComponentManager _componentManager;
    private GameManager _gameManager;
    private PlayerManager _playerManager;
    private UIManager _uiManager;
    private MotoRigController _rigController;

    [BoxGroup("Player Options")]
    public PlayerMovementController playerMovementController;
    [BoxGroup("Male Player")]
    public GameObject MalePlayer;
    [BoxGroup("Female Player")]
    public GameObject FemalePlayer;

    [BoxGroup("Player Value Options")]
    public int PlayerDamageCount = 1;
    [BoxGroup("Player Value Options")]
    public int CollectedCoinCount = 1;

    [BoxGroup("Player Value Options")]
    public int CollectedCrystalCount = 1;

    [Header("Character Controller Required")]
    [BoxGroup("Player Jump Value Options")]
    public float JumpForce;
    [BoxGroup("Player Value Options")]
    public float JumpHeight;

    public Tweener MoveZ1;
    public Tweener MoveZ2;

    [BoxGroup("Booleans")]
    public bool alreadyBumped;
    public bool withinSprayArea=false;
    public bool sprayAvailable = true;

    [BoxGroup("Particle Systems")]
    public ParticleSystem sadEmoji;
    public ParticleSystem copEmoji;
    public ParticleSystem dogEmoji;
    public ParticleSystem pickupSparks;
    public ParticleSystem bumpWave;

    [BoxGroup("Spray Objects")]
    public GameObject sprayParticle;
    public GameObject sprayGO;

    [BoxGroup("Spray Fall Phase Objects")]
    public GameObject playerHand;
    public GameObject fallSpray;

    public GameObject finishPainter;
    private float TimerSliderAmount = 1.25f;

    public GameObject motorcyclePlayer;


    GameObject maleLand;// = GameObject.Find("maleLand");
    GameObject femaleLand;// = GameObject.Find("femaleLand");
    private void Awake()
    {
        _collectManager = CollectManager.Instance;
        _gameManager = GameManager.Instance;
        _playerManager = PlayerManager.Instance;
        _componentManager = ComponentManager.Instance;
        _uiManager = UIManager.Instance;
        //_rigController = MotoRigController.Instance;

        
    }

    private void Update()
    {
        if(withinSprayArea && !sprayAvailable)
        {
            _playerManager.MalePlayerAnimationController.SetBool("isBombing", false);
            _playerManager.FemalePlayerAnimationController.SetBool("isBombing", false);
        }

        if(withinSprayArea && sprayAvailable && !alreadyBumped && _componentManager.TimerSliderComponent.value>0)
        {
            sprayParticle.SetActive(true);
            _componentManager.TimerSliderComponent.value -= TimerSliderAmount * Time.deltaTime;
        }
        else if(!withinSprayArea || !sprayAvailable || alreadyBumped || _componentManager.TimerSliderComponent.value <= 0)
        {
            sprayParticle.SetActive(false);
        }
    }

    public async void BumpedPhase()
    {  
        Vibration.VibratePop();

        _playerManager.PlayerSpeed = 0;
        _playerManager.MalePlayerAnimationController.SetBool("isBumped", true);
        _playerManager.FemalePlayerAnimationController.SetBool("isBumped", true);

        MoveZ1 = MalePlayer.transform.DOMove(new Vector3(MalePlayer.transform.position.x, MalePlayer.transform.position.y, MalePlayer.transform.position.z - 6f), 1f);
        MoveZ2= FemalePlayer.transform.DOMove(new Vector3(FemalePlayer.transform.position.x, FemalePlayer.transform.position.y, FemalePlayer.transform.position.z - 6f), 1f);

        await Task.Delay(1000);

        alreadyBumped = false;
        _playerManager.MalePlayerAnimationController.SetBool("isBumped", false);
        _playerManager.FemalePlayerAnimationController.SetBool("isBumped", false);
        _playerManager.PlayerSpeed = _playerManager.DefaultPlayerSpeed;
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            Vibration.VibratePop();

            //mainCameraLookAt.transform.DOMove(other.transform.GetChild(2).position, 1);

            if (other.GetComponent<DeadOnHit>())
            {
                _playerManager.AddDamage(_playerManager.PlayerMaxDeadCount);
                _collectManager.ObstacleObjects.Add(other.gameObject);
                //other.gameObject.SetActive(false);
            }
            else{
                _playerManager.AddDamage(PlayerDamageCount);
                _collectManager.ObstacleObjects.Add(other.gameObject);
                //other.gameObject.SetActive(false);
            }
           
        }

        if(other.tag=="Jumper")
        {
            maleLand = other.transform.GetChild(0).gameObject;
            femaleLand = other.transform.GetChild(1).gameObject;
            JumpPhase();
          

        }

        if(other.tag=="Police")
        {

            var policeSpot = other.gameObject.transform.Find("policeSpot");

            this.transform.position = policeSpot.transform.position;

            _playerManager.AddPoliceDamage(PlayerDamageCount);
            _collectManager.ObstacleObjects.Add(other.gameObject);
            copEmoji.Play();

        }

        if (other.tag == "Motocop")
        {
            _rigController = MotoRigController.Instance;

            var policeSpot = other.gameObject.transform.Find("policeSpot");

            MalePlayer.transform.position = policeSpot.transform.position;

            _rigController.MotorHandsUp();

            _playerManager.AddPoliceDamage(PlayerDamageCount);
            _collectManager.ObstacleObjects.Add(other.gameObject);
            //copEmoji.Play();

        }

        if (other.tag=="Dog")
        {
            var maleSpot = other.gameObject.transform.Find("maleSpot");
            var femaleSpot = other.gameObject.transform.Find("femaleSpot");

            MalePlayer.transform.position = maleSpot.transform.position;
            FemalePlayer.transform.position = femaleSpot.transform.position;
            sprayGO.SetActive(false);

            _playerManager.AddDogDamage(PlayerDamageCount);
            _collectManager.ObstacleObjects.Add(other.gameObject);
            dogEmoji.Play();
            
        }

        if(other.tag=="Hole")
        {
            _playerManager.PlayerSpeed = 0;
            _playerManager.PlayerSideSpeed = 0;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            this.gameObject.GetComponent<Rigidbody>().useGravity = true;

            _playerManager.MalePlayerAnimationController.SetBool("isLose", true);
            _playerManager.FemalePlayerAnimationController.SetBool("isLose", true);
        }

        if (other.tag == "FallTrigger")
        {

            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            _gameManager.UpdateGameState(GameState.LoseGame);
        }

        if (other.tag == "BumpWall")
        {
            alreadyBumped = true;
            bumpWave.Play();

            if (_playerManager.bumpCount < 2 && alreadyBumped && sprayAvailable)
            {
                sprayAvailable = false;
                sprayGO.SetActive(false);

                Vector3 playerHandPos = playerHand.transform.position;
                Quaternion playerHandrot = playerHand.transform.rotation;
                _componentManager.TimerSliderComponent.value = 0;
                var clone = Instantiate(fallSpray, playerHandPos, playerHandrot);
                Destroy(clone, 6.0f);

                
            }

            if (_playerManager.bumpCount <= 5 && alreadyBumped)
            {
                BumpedPhase();
                _playerManager.bumpCount++;
            }
            else
            {
                MoveZ1.Kill();
                MoveZ2.Kill();
                sadEmoji.Play();
                _gameManager.UpdateGameState(GameState.LoseGame);
                
            }
        }

        if (other.tag == "BumpCounterReset")
        {
            _playerManager.bumpCount = 0;
        }

        if (other.tag == "Collectable" || other.tag=="Spray")
        {
            sprayAvailable = true;
            if(!motorcyclePlayer.activeInHierarchy)
            {
            sprayGO.SetActive(true);
            }
            

            _collectManager.CollectedObjects.Add(other.gameObject);
            other.gameObject.SetActive(false);
            pickupSparks.Play();
            


            if (other.GetComponent<CrystalMode>())
            {
                _collectManager.AddCrystal(CollectedCrystalCount);
                _collectManager.CollectedObjects.Add(other.gameObject);
                other.gameObject.SetActive(false);
            }
            else
            {
                other.gameObject.SetActive(false);

                if (other.tag == "Collectable")
                {
                    _collectManager.AddCoin(CollectedCoinCount);

                }


            }

            if (other.GetComponent<TimeMode>())
            {
                if (other.GetComponent<TimeMode>().isIncrease)
                {
                    _gameManager.UpdateGameTimeState(TimeState.AddTime, other.GetComponent<TimeMode>().IncreaseTimeAmount);
                    _collectManager.CollectedObjects.Add(other.gameObject);
                    other.gameObject.SetActive(false);
                }
                else
                {
                    _gameManager.UpdateGameTimeState(TimeState.DecreaseTime, other.GetComponent<TimeMode>().DecreaseTimeAmount);
                    _collectManager.CollectedObjects.Add(other.gameObject);
                    other.gameObject.SetActive(false);
                }
            }
        }

        if(other.tag=="FinishLine")
        {
            _playerManager.PlayerSpeed = 0;
            _playerManager.PlayerSideSpeed = 0;
            _uiManager.GamePanel.SetActive(false);
            var detectionFinish = GameObject.Find("DetectionFinish");

            MalePlayer.transform.DOMove(new Vector3(-2, MalePlayer.transform.position.y, detectionFinish.transform.position.z), 1.25f);
            FemalePlayer.transform.DOMove(new Vector3(2, FemalePlayer.transform.position.y, detectionFinish.transform.position.z), 1.25f);


        }

        if (other.tag == "motorcycleEnd")
        {
            EndJumpPhase();
        }

        if (other.tag == "Finish")
        {
            FinishPhase();
        }

        if(other.tag=="MiddleRoad")
        {
            withinSprayArea = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "SprayTrigger")
        {
            withinSprayArea = true;

            if(_componentManager.TimerSliderComponent.value > 0)
            {
            _playerManager.MalePlayerAnimationController.SetBool("isBombing", true);
            _playerManager.FemalePlayerAnimationController.SetBool("isBombing", true);
            }
            else if(_componentManager.TimerSliderComponent.value <= 0)
            {
                _playerManager.MalePlayerAnimationController.SetBool("isBombing", false);
                _playerManager.FemalePlayerAnimationController.SetBool("isBombing", false);
            }
          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SprayTrigger")
        {
            withinSprayArea = false;

            _playerManager.MalePlayerAnimationController.SetBool("isBombing", false);
            _playerManager.FemalePlayerAnimationController.SetBool("isBombing", false);
        }
    }

    public async void FinishPhase()
    {
        _playerManager.PlayerSpeed = 0;
        _playerManager.PlayerSideSpeed = 0;

        _playerManager.MalePlayerAnimationController.SetBool("isFinished", true);
        _playerManager.FemalePlayerAnimationController.SetBool("isFinished", true);

        MalePlayer.transform.DOMove(new Vector3(MalePlayer.transform.position.x +1.5f, MalePlayer.transform.position.y, MalePlayer.transform.position.z), 4f);
        FemalePlayer.transform.DOMove(new Vector3(FemalePlayer.transform.position.x - 1.5f, FemalePlayer.transform.position.y, FemalePlayer.transform.position.z), 4f);

        await Task.Delay(3500);

        _gameManager.UpdateGameState(GameState.WinGame);

    }

    public async void ActivateP3D()
    {
        while(motorcyclePlayer.activeInHierarchy && _componentManager.TimerSliderComponent.value>0)
        {
            await Task.Delay(300);
            _componentManager.P3DPainter.SetActive(false);
            await Task.Delay(25);
            _componentManager.P3DPainter.SetActive(true);

        }
    }

    public async void JumpPhase()
    {


        MalePlayer.transform.DOJump(maleLand.transform.position, 4, 1, 1f, false);
        FemalePlayer.transform.DOJump(femaleLand.transform.position, 3, 1, 1f, false);
        
        _playerManager.MalePlayerAnimationController.SetBool("isMotorJump", true);
        _playerManager.FemalePlayerAnimationController.SetBool("isMotorJump", true);

        await Task.Delay(1000);

        MalePlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        FemalePlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        GameObject.Find("Hair").GetComponent<SkinnedMeshRenderer>().enabled = false;
        GameObject.Find("Kirpik").GetComponent<SkinnedMeshRenderer>().enabled = false;
        GameObject.Find("Left_Shoes").GetComponent<SkinnedMeshRenderer>().enabled = false;

        
        _playerManager.MalePlayerAnimationController.SetBool("isMotorJump", false);
        _playerManager.FemalePlayerAnimationController.SetBool("isMotorJump", false);

        _playerManager.PlayerSpeed = _playerManager.DefaultPlayerSpeed + 5;
        _playerManager.PlayerSideSpeed = 15;

        sprayGO.SetActive(false);
        motorcyclePlayer.SetActive(true);
        ActivateP3D();
    }

    public async void EndJumpPhase()
    {
        var endMaleLand = GameObject.Find("endMaleLand");
        var endFemaleLand = GameObject.Find("endFemaleLand");

        motorcyclePlayer.SetActive(false);

        MalePlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        FemalePlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        GameObject.Find("Hair").GetComponent<SkinnedMeshRenderer>().enabled = true;
        GameObject.Find("Kirpik").GetComponent<SkinnedMeshRenderer>().enabled = true;
        GameObject.Find("Left_Shoes").GetComponent<SkinnedMeshRenderer>().enabled = true;

        MalePlayer.transform.DOJump(endMaleLand.transform.position, 3, 1, 1f, false);
        FemalePlayer.transform.DOJump(endFemaleLand.transform.position, 3, 1, 1f, false);

        _playerManager.MalePlayerAnimationController.SetBool("isMotorJump", true);
        _playerManager.FemalePlayerAnimationController.SetBool("isMotorJump", true);

        await Task.Delay(1000);

        _playerManager.MalePlayerAnimationController.SetBool("isMotorJump", false);
        _playerManager.FemalePlayerAnimationController.SetBool("isMotorJump", false);

        _playerManager.PlayerSpeed = _playerManager.DefaultPlayerSpeed;
        _playerManager.PlayerSideSpeed = 10;

        sprayGO.SetActive(true);
        
    }

}
