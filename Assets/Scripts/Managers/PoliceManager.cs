using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using NaughtyAttributes;
using DG.Tweening;

public class PoliceManager : MonoBehaviour
{
    public ParticleSystem angy;
    public GameObject policeCamera;
    public GameObject lookAt;

    private PlayerManager _playerManager;
    private CollectManager _collectManager;
    private void Awake()
    {
        _playerManager = PlayerManager.Instance;
        _collectManager = CollectManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Hitter")
        {
            PolicePhasing();
        }
        
    }
    public async void PolicePhasing()
    {
        var cm = GameObject.Find("CM_CameraLook").transform;

        policeCamera.transform.position = new Vector3(0, cm.transform.position.y, cm.transform.position.z + 5);
        policeCamera.transform.rotation = cm.transform.rotation;
        policeCamera.SetActive(true);
        policeCamera.transform.DOMove(lookAt.transform.position, 1.5f);
        policeCamera.transform.DORotate(new Vector3(lookAt.transform.eulerAngles.x, lookAt.transform.eulerAngles.y, lookAt.transform.eulerAngles.z), 1.5f);
     
        await Task.Delay(1200);
        angy.Play();
    }
}
