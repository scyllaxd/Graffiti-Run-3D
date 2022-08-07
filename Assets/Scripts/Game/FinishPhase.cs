using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FinishPhase : MonoBehaviour
{
    public Image amazing1;
    public Image amazing2;
    public bool playerDidHitFinish = false;

    public GameObject finishCamera;
    public GameObject lookAt;
    public ParticleSystem confetti;
    public ParticleSystem finishSmoke;

    private PlayerManager _playerManager;

    private void Awake()
    {
        _playerManager = PlayerManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hitter")
        {
            playerDidHitFinish = true;
            confetti.Play();
            var cm = GameObject.Find("CM_CameraLook").transform;
            finishCamera.transform.position = new Vector3(0, cm.transform.position.y, cm.transform.position.z + 5);
            finishCamera.transform.rotation = cm.transform.rotation;
            finishCamera.SetActive(true);
            finishSmoke.Play();
            finishCamera.transform.DOMove(lookAt.transform.position, 5f);
            finishCamera.transform.DORotate(new Vector3(lookAt.transform.eulerAngles.x, lookAt.transform.eulerAngles.y, lookAt.transform.eulerAngles.z), 5f);

        }
    }


    void Update()
    {
        if(playerDidHitFinish)
        {
        amazing1.fillAmount += 0.29f * Time.deltaTime;
        amazing2.fillAmount += 0.29f * Time.deltaTime;
        }
        
    }
}
