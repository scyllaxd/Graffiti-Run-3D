/* Author : Mehmet Bedirhan U?ak*/

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;

public class CollectManager : Singleton<CollectManager>
{
    [BoxGroup("Collected Coin and Total Coin")]
    public int CollectedCoin;
    [BoxGroup("Collected Coin and Total Coin")]
    public int CollectedCrystal;
    [BoxGroup("Time Amount Options")]
    public float CollectedTimeIncraseAmount;
    [BoxGroup("Collected Coin and Total Coin")]
    public float CollectedTimeDecraseAmount;
    [BoxGroup("Collected List Obeject")]
    public List<GameObject> CollectedObjects = new List<GameObject>();
    [BoxGroup("Obstacle List Obeject")]
    public List<GameObject> ObstacleObjects = new List<GameObject>();

    public void AddCoin(int amount)
    {
        CollectedCoin += amount;
    }

    public void AddCrystal(int amount)
    {
        CollectedCrystal += amount;
    }

    public void AddTimeAmount(float amount)
    {
        CollectedTimeIncraseAmount += amount;
    }

    public void DecraseTimeAmount(float amount)
    {
        CollectedTimeDecraseAmount -= amount;
    }

    public async void ActiveCollectedObject()
    {
        for (int i = 0; i < CollectedObjects.Count; i++)
        {
            CollectedObjects[i].SetActive(true);
        }
        await Task.Delay(100);
        for (int i = CollectedObjects.Count - 1; i >= 0; i--)
        {
            CollectedObjects.RemoveAt(i);
        }
    }

    public async void ActiveObstacleObject()
    {
        for (int i = 0; i < ObstacleObjects.Count; i++)
        {
            ObstacleObjects[i].SetActive(true);
        }
        await Task.Delay(100);
        for (int i = ObstacleObjects.Count - 1; i >= 0; i--)
        {
            ObstacleObjects.RemoveAt(i);
        }
    }
}
