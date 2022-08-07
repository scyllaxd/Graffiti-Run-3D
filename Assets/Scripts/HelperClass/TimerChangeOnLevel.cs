
using UnityEngine;
using NaughtyAttributes;
public class TimerChangeOnLevel : MonoBehaviour
{
    private GameManager _gameManager;
    public float SetLevelTime = 10f;
    public bool isLevelTimeEnabled;
    private float MaxLevelTime;
    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        if (isLevelTimeEnabled)
        {
            MaxLevelTime = SetLevelTime;
            _gameManager.UpdateGameMaxTimeState(TimeState.HandleSetMaxTime, MaxLevelTime);
        }
        
    }

    private void OnEnable()
    {
        if (isLevelTimeEnabled)
        {
            MaxLevelTime = SetLevelTime;
            _gameManager.UpdateGameMaxTimeState(TimeState.HandleSetMaxTime, MaxLevelTime);
        }
    }


    // Debug Test Class
    [Button("Add Time")]
    public void AddTime()
    {
        _gameManager.UpdateGameTimeState(TimeState.AddTime, 5f);
    }
    [Button("Decrease Time")]
    public void DecreaseTime()
    {
        _gameManager.UpdateGameTimeState(TimeState.DecreaseTime, 5f);
    }

}
