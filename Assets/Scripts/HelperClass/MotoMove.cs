/* Author : Mehmet Bedirhan U?ak*/
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using System.Threading.Tasks;

public class MotoMove : MonoBehaviour
{
    private float? lastMousePoint = null;
    [BoxGroup("Player Options ")]
    public float RestirictionX = 3.5f;
    [BoxGroup("Player Mesh Transform")]
    public GameObject PlayerHolder;
    [BoxGroup("Camera Options")]
    public GameObject PlayerFollower;
    [BoxGroup("Camera Options")]
    public bool isFollowPlayer;
    [BoxGroup("Camera Options")]
    public float OffsetY, OffsetX, OffsetZ;
    [BoxGroup("Camera Options")]
    public Transform FollowerObjectCamera;
    private Vector3 oldPos;
    private Quaternion oldRot;
    public bool mouseControl;
    private PlayerManager _playerManager;

    [BoxGroup("Rotation Mode Options")]
    public bool PlayerRotateEnabled;
    [BoxGroup("Rotation Mode Options")]
    public float RotatationDegree;
    [BoxGroup("Rotation Mode Options")]
    public float RotationSpeed;
    [BoxGroup("Lean Rotation Degree")]
    public float LeanRotatationDegree;
    private float _oldPosition;
    public float motorSpeed = 10f;
    public float motorSideSpeed = 50f;

    public GameObject motorParent;

    private CharacterController _characterController;


    private void Awake()
    {
        _playerManager = PlayerManager.Instance;
        //_characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        
    //PlayerFollower.gameObject.transform.parent = this.transform;

    //_characterController.enabled = false;
    }

    private void LateUpdate()
    {
        /*_oldPosition = PlayerHolder.transform.localPosition.x;
        if (isFollowPlayer)
        {
            if (FollowerObjectCamera != null)
                PlayerFollower.transform.position = new Vector3(FollowerObjectCamera.transform.position.x + OffsetX, FollowerObjectCamera.transform.position.y + OffsetY, FollowerObjectCamera.transform.position.z + OffsetZ);
        }*/

    }

    private void Update()
    {
        /*transform.Translate(Vector3.forward * Time.deltaTime * motorSpeed);

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
                PlayerHolder.transform.position = new Vector3(PlayerHolder.transform.position.x + (difference / 188) * Time.deltaTime * motorSideSpeed, PlayerHolder.transform.position.y, PlayerHolder.transform.position.z);
                lastMousePoint = Input.mousePosition.x;
            }

            float xPos = Mathf.Clamp(PlayerHolder.transform.position.x, -RestirictionX, RestirictionX);
            PlayerHolder.transform.position = new Vector3(xPos, PlayerHolder.transform.position.y, PlayerHolder.transform.position.z);
            Vector3 movement = oldRot * (PlayerHolder.transform.position - oldPos);*/

            if (PlayerRotateEnabled)
            {
                if (motorParent.transform.localPosition.x > _oldPosition)
                {
                    PlayerHolder.transform.DORotate(new Vector3(0f, RotatationDegree, 0f), RotationSpeed);
                    PlayerHolder.transform.DORotate(new Vector3(0f, 0f, -LeanRotatationDegree), RotationSpeed);
                }

                else if (motorParent.transform.localPosition.x < _oldPosition)
                {
                    PlayerHolder.transform.DORotate(new Vector3(0f, -RotatationDegree, 0f), RotationSpeed);
                    PlayerHolder.transform.DORotate(new Vector3(0f, 0f, LeanRotatationDegree), RotationSpeed);
                }
                else
                {
                    PlayerHolder.transform.DORotate(new Vector3(0f, 0f, 0f), RotationSpeed);
                }
                _oldPosition = motorParent.transform.localPosition.x;
            }

        }
    }

