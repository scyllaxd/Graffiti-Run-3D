using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerMovementJoypadController : MonoBehaviour
{

    [BoxGroup("Movement Options")]
    public bool isCharacterControllerEnabled;
    [BoxGroup("Speed Options")]
    public float Speed;
    private float _speed;
    [BoxGroup("Speed Options")]
    public bool PlayerisRunning;
    [BoxGroup("Camera Options")]
    public GameObject PlayerFollower;
    [BoxGroup("Camera Options")]
    public float OffsetY,OffsetX,OffsetZ;
    
    public FloatingJoystick floatingJoystick;
    private PlayerManager _playerManager;
    private CharacterController _characterController;

    private void Awake()
    {
        _playerManager = PlayerManager.Instance;
        _characterController = GetComponent<CharacterController>();
        _speed = Speed;
    }

    private void Start()
    {
        PlayerFollower.gameObject.transform.parent = null;
        if(isCharacterControllerEnabled)
            _characterController.enabled = true;
        else
            _characterController.enabled = false;
    }

    public void FixedUpdate()
    {

        Vector3 direction = Vector3.forward * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
        Quaternion rotation = Quaternion.LookRotation(direction);

        PlayerFollower.transform.position = new Vector3(transform.position.x + OffsetX, transform.position.y + OffsetY, transform.position.z + OffsetZ);


        if (floatingJoystick.Vertical == 0 && floatingJoystick.Horizontal == 0)
        {
            Speed = 0;
            _playerManager.IdleStatePlayer();
        }
        else
        {
            _playerManager.StartPlayer(!PlayerisRunning);
            Speed = _speed;
            transform.rotation = rotation;
            if(isCharacterControllerEnabled)
                _characterController.Move(direction * Time.deltaTime * Speed);
            else
                transform.Translate(Vector3.forward * Speed * Time.fixedDeltaTime);
                
        }
    }


}
