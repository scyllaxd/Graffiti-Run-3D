using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DogManager : MonoBehaviour
{
    public GameObject dogCamera;
    public GameObject lookAt;
    public Animator dogAnimationController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hitter")
        {
            var cm = GameObject.Find("CM_CameraLook").transform;
            dogCamera.transform.position = new Vector3(0, cm.transform.position.y, cm.transform.position.z + 5);
            dogCamera.transform.rotation = cm.transform.rotation;
            dogCamera.SetActive(true);
            dogCamera.transform.DOMove(lookAt.transform.position, 1.5f);
            dogCamera.transform.DORotate(new Vector3(lookAt.transform.eulerAngles.x, lookAt.transform.eulerAngles.y, lookAt.transform.eulerAngles.z), 1.5f);

            dogAnimationController.SetBool("triggerDog", true);

        }

    }
}
