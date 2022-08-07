/* Author : Mehmet Bedirhan U?ak*/
using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;
public class DebugGame : MonoBehaviour
{
    public GameObject Player;
    private GameObject Finish;
    private void Update()
    {
        Finish = GameObject.FindGameObjectWithTag("Finish");
    }

    [Button("Go to Finish")]
    public async void GoToFinish()
    {
        if(Finish != null)
        {
            Player.transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = false;
            await Task.Delay(250);
            Player.transform.position = Finish.transform.position;
            Player.transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            Debug.LogWarning("Finish is not Set!!");
        }   
    }
    [Button("Clear All Data")]
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    [Button("Clear System Time Data")]
    public void DeleteTimeData()
    {
        PlayerPrefs.DeleteKey("SystemTime");
    }

}
