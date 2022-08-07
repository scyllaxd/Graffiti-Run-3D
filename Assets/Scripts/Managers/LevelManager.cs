/* Author : Mehmet Bedirhan U?ak*/
using UnityEngine;
using NaughtyAttributes;

public class LevelManager : Singleton<LevelManager>
{

    private CollectManager _collectManager;
    private GameManager _gameManager;
    [BoxGroup("Level Options")]
    public int LevelNumber;
    [BoxGroup("Level Options")]
    [HideInInspector]
    public int MaxLevel;
    [BoxGroup("Level Options")]
    public int DisplayLevelNumer = 1;
    private int _coin;
    private int _crystal;
    [BoxGroup("Level Prefab Options")]
    public GameObject LevelHolder;
    [HideInInspector]
    public GameObject[] SpawnedLevels;
    [BoxGroup("Level Prefab Options")]
    public GameObject[] Levels;

    
    
    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _collectManager = CollectManager.Instance;
        MaxLevel = Levels.Length - 1;
    }

    void Start()
    {
        LoadLevel();
        NetLevelCall();
    }

    public void NextLevel()
    {
        if (LevelNumber == MaxLevel)
        {
            LevelNumber = 0;
        }
        else
        {
            LevelNumber++;
        }
        DisplayLevelNumer++;
        SaveLevel();
        NetLevelCall();
    }

    public void NetLevelCall()
    {
        SpawnedLevels = GameObject.FindGameObjectsWithTag("Level");
        for (int i = 0; i < SpawnedLevels.Length; i++)
        {
            Destroy(SpawnedLevels[i].gameObject);
        }

        Instantiate(Levels[LevelNumber].gameObject, LevelHolder.transform);

    }
    [Button("Restart Level Debug")]
    public void RestartLevel()
    {
        
        SpawnedLevels = GameObject.FindGameObjectsWithTag("Level");
        for (int i = 0; i < SpawnedLevels.Length; i++)
        {
            Destroy(SpawnedLevels[i].gameObject);
        }
        Instantiate(Levels[LevelNumber].gameObject, LevelHolder.transform);

    }


    public void SaveLevel()
    {
        PlayerPrefs.SetInt("Level", LevelNumber);
        PlayerPrefs.SetInt("LevelNumber", DisplayLevelNumer);
        PlayerPrefs.SetInt("Coin", _collectManager.CollectedCoin);
        PlayerPrefs.SetInt("Cystal", _collectManager.CollectedCrystal);
    }

    public void LoadLevel()
    {
        if (PlayerPrefs.HasKey("Coin"))
        {
            _coin = PlayerPrefs.GetInt("Coin");
            _collectManager.CollectedCoin = _coin;
        }

        if (PlayerPrefs.HasKey("Cystal"))
        {
            _crystal = PlayerPrefs.GetInt("Cystal");
            _collectManager.CollectedCrystal = _crystal;
        }
        if (PlayerPrefs.HasKey("Level"))
        {
            LevelNumber = PlayerPrefs.GetInt("Level");
        }
        if (PlayerPrefs.HasKey("LevelNumber"))
        {
            DisplayLevelNumer = PlayerPrefs.GetInt("LevelNumber");
        }

        if (DisplayLevelNumer > 1)
        {
            //_gameManager.UpdateGameState(GameState.StartGame);
            Time.timeScale = 1;
        }
    }
}
