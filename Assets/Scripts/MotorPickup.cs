using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorPickup : MonoBehaviour
{
    public GameObject pickupCloud;
    public GameObject chestOffset;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Hitter")
        {
            var clone = Instantiate(pickupCloud, chestOffset.transform.position, Quaternion.identity);
            Destroy(clone, 3);
        }
    }
}
