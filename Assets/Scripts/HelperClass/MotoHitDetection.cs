using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using NaughtyAttributes;
using DG.Tweening;

public class MotoHitDetection : MonoBehaviour
{
    private CollectManager _collectManager;
    private ComponentManager _componentManager;
    private GameManager _gameManager;
    private PlayerManager _playerManager;
    private UIManager _uiManager;
    private HitDetection _hitDetection;

    public ParticleSystem pickupSparks;

    private void Awake()
    {
        _collectManager = CollectManager.Instance;
        _gameManager = GameManager.Instance;
        _playerManager = PlayerManager.Instance;
        _componentManager = ComponentManager.Instance;
        _uiManager = UIManager.Instance;
        _hitDetection = HitDetection.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        //MotorcyclePhasing();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectable" || other.tag == "Spray")
        {
            _hitDetection.sprayAvailable = true;
            //sprayGO.SetActive(true);

            _collectManager.CollectedObjects.Add(other.gameObject);
            other.gameObject.SetActive(false);
            pickupSparks.Play();

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
}

