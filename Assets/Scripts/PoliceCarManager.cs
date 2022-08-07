using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarManager : MonoBehaviour
{
    public GameObject redLight;
    public GameObject blueLight;
    void Start()
    {
     StartCoroutine(Lights());
    }

    void Update()
    {
        
    }

    IEnumerator Lights()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.2f);
            blueLight.SetActive(false);
            redLight.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            redLight.SetActive(false);
            blueLight.SetActive(true);
        }

    }
}
