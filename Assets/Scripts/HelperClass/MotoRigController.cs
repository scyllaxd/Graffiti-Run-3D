using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MotoRigController : Singleton<MotoRigController>
{
    private PlayerManager _playerManager;
    private PlayerMovementController _playerMovementController;
    
    public GameObject maleRig;
    public GameObject femaleRig;

    public GameObject maleCaughtRig;
    public GameObject femaleCaughtRig;

    public GameObject maleSpray;
    public GameObject femaleSpray;

    public bool isFailed = false;

    private void Awake()
    {
        _playerManager = PlayerManager.Instance;
        _playerMovementController = PlayerMovementController.Instance;

    }

    void Update()
    {
        if(!isFailed)
        {
            

            if (_playerMovementController.PlayerHolder.transform.position.x < -1.4f)
            {
                maleRig.GetComponent<TwoBoneIKConstraint>().weight = 1;

                if(maleCaughtRig.GetComponent<Rig>().weight==0)
                {
                    maleSpray.SetActive(true);
                }
                
            }
            else if (_playerMovementController.PlayerHolder.transform.position.x > 1.3f)
            {
                femaleRig.GetComponent<TwoBoneIKConstraint>().weight = 1;

                if (femaleCaughtRig.GetComponent<Rig>().weight == 0)
                {
                    femaleSpray.SetActive(true);
                }
            }
            else
            {
                maleCaughtRig.GetComponent<Rig>().weight = 0;
                femaleCaughtRig.GetComponent<Rig>().weight = 0;

                maleRig.GetComponent<TwoBoneIKConstraint>().weight = 0;
                femaleRig.GetComponent<TwoBoneIKConstraint>().weight = 0;

                maleSpray.SetActive(false);
                femaleSpray.SetActive(false);
            }
        }
        
    }

    public void MotorHandsUp()
    {
        isFailed = true;
        maleCaughtRig.GetComponent<Rig>().weight = 1;
        femaleCaughtRig.GetComponent<Rig>().weight = 1;
        maleSpray.SetActive(false);
        femaleSpray.SetActive(false);
        isFailed = false;
    }

    public void ResetAllRig()
    {
        maleCaughtRig.GetComponent<Rig>().weight = 0;
        femaleCaughtRig.GetComponent<Rig>().weight = 0;
        maleRig.GetComponent<TwoBoneIKConstraint>().weight = 0;
        femaleRig.GetComponent<TwoBoneIKConstraint>().weight = 0;
    }
}
