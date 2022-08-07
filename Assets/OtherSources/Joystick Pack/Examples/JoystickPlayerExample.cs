using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public void FixedUpdate()
    {

        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        Quaternion rotation = Quaternion.LookRotation(direction);
       


        if (variableJoystick.Vertical == 0 && variableJoystick.Horizontal == 0)
        {
            speed = 0;
        }
        else
        {
            speed = 8;
            transform.rotation = rotation;
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        }
    }
}