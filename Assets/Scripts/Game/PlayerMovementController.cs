/* Author : Mehmet Bedirhan U?ak*/
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using PaintIn3D;

public class PlayerMovementController : Singleton<PlayerMovementController>
{

    private float? lastMousePoint = null;
    [BoxGroup("Player Options ")]
    public float RestirictionX = 2f;
    public float midClampPoint = 0.4f;
    [BoxGroup("Player Mesh Transform")]
    public GameObject PlayerHolder;
    public GameObject PlayerHolder2;
    [BoxGroup("Camera Options")]
    public GameObject PlayerFollower;
    private Vector3 oldPos;
    private Quaternion oldRot;
    private bool mouseControl;
    private PlayerManager _playerManager;

    [BoxGroup("Rotation Mode Options")]
    public bool PlayerRotateEnabled;
    [BoxGroup("Rotation Mode Options")]
    public float RotatationDegree;
    [BoxGroup("Rotation Mode Options")]
    public float RotationSpeed;
    private float _oldPosition;
    private float _oldPosition2;
    P3dInputManager p3DInputManager;
    private CharacterController _characterController;
    public GameObject motorGO;
    public bool onMotor = false;


    private void Awake()
    {
        _playerManager = PlayerManager.Instance;
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        PlayerFollower.gameObject.transform.parent = this.transform;
        _characterController.enabled = false;
        p3DInputManager = FindObjectOfType<P3dInputManager>();
    }

    private void LateUpdate()
    {
        
    }

    private void Update()
    {

        if (!motorGO.activeInHierarchy)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _playerManager.PlayerSpeed);

            if (!mouseControl)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    lastMousePoint = Input.mousePosition.x;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    lastMousePoint = null;
                }
                if (lastMousePoint != null)
                {
                    float difference = Input.mousePosition.x - lastMousePoint.Value;
                    PlayerHolder.transform.position = new Vector3(PlayerHolder.transform.position.x + (difference / 188) * Time.deltaTime * _playerManager.PlayerSideSpeed, PlayerHolder.transform.position.y, PlayerHolder.transform.position.z);
                    PlayerHolder2.transform.position = new Vector3(PlayerHolder2.transform.position.x - (difference / 188) * Time.deltaTime * _playerManager.PlayerSideSpeed, PlayerHolder2.transform.position.y, PlayerHolder2.transform.position.z);
                    lastMousePoint = Input.mousePosition.x;
                }

                float xPos = Mathf.Clamp(PlayerHolder.transform.position.x, -RestirictionX, -midClampPoint);
                float xPos2 = Mathf.Clamp(PlayerHolder2.transform.position.x, midClampPoint, RestirictionX);

                PlayerHolder.transform.position = new Vector3(xPos, PlayerHolder.transform.position.y, PlayerHolder.transform.position.z);
                PlayerHolder2.transform.position = new Vector3(xPos2, PlayerHolder2.transform.position.y, PlayerHolder2.transform.position.z);

                Vector3 movement = oldRot * (PlayerHolder.transform.position - oldPos);
                Vector3 movement2 = oldRot * (PlayerHolder2.transform.position - oldPos);
            }

        }
            else if(motorGO.activeInHierarchy)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * _playerManager.PlayerSpeed);

                if (PlayerHolder.transform.position.x > 1.2f)
                {
                var controller = motorGO.GetComponent<CharacterController>();
                controller.center = new Vector3(-1.25f, 0, 0);
                }
                else if(PlayerHolder.transform.position.x < -1.4f)
                {
                var controller = motorGO.GetComponent<CharacterController>();
                controller.center = new Vector3(1.25f, 0, 0);
                }
                /*else
                {
                var controller = motorGO.GetComponent<CharacterController>();
                controller.center = new Vector3(0, 0, 0);
                }*/

            if (!mouseControl)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        lastMousePoint = Input.mousePosition.x;
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        lastMousePoint = null;
                    }
                    if (lastMousePoint != null)
                    {
                        float difference = Input.mousePosition.x - lastMousePoint.Value;
                        PlayerHolder.transform.position = new Vector3(PlayerHolder.transform.position.x + (difference / 188) * Time.deltaTime * _playerManager.PlayerSideSpeed, PlayerHolder.transform.position.y, PlayerHolder.transform.position.z);
                        PlayerHolder2.transform.position = new Vector3(PlayerHolder2.transform.position.x + (difference / 188) * Time.deltaTime * _playerManager.PlayerSideSpeed, PlayerHolder2.transform.position.y, PlayerHolder2.transform.position.z);
                        lastMousePoint = Input.mousePosition.x;
                    }

                    float xPos = Mathf.Clamp(PlayerHolder.transform.position.x, -RestirictionX, RestirictionX-0.4f);
                    float xPos2 = Mathf.Clamp(PlayerHolder2.transform.position.x, -RestirictionX, RestirictionX);

                PlayerHolder.transform.position = new Vector3(xPos, PlayerHolder.transform.position.y, PlayerHolder.transform.position.z);
                PlayerHolder2.transform.position = new Vector3(xPos2, PlayerHolder2.transform.position.y, PlayerHolder2.transform.position.z);

                Vector3 movement = oldRot * (PlayerHolder.transform.position - oldPos);
                Vector3 movement2 = oldRot * (PlayerHolder2.transform.position - oldPos);
                }
            }
        

        






            if (PlayerRotateEnabled)
            {
                if (PlayerHolder.transform.localPosition.x > _oldPosition)
                {
                    PlayerHolder.transform.DORotate(new Vector3(0f, -RotatationDegree, 0f), RotationSpeed);
                }

                else if (PlayerHolder.transform.localPosition.x < _oldPosition)
                {
                    PlayerHolder.transform.DORotate(new Vector3(0f, RotatationDegree, 0f), RotationSpeed);
                }
                else
                {
                    PlayerHolder.transform.DORotate(new Vector3(0f, 0f, 0f), RotationSpeed);
                }
                _oldPosition = PlayerHolder.transform.localPosition.x;
            }

        }
    }
