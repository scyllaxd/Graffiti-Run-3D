using UnityEngine;

public class CamFollow : MonoBehaviour

{
    public Transform target;
    public float speed = 10f;
    Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    // follows the player smoothly
    void Update()
    {
        transform.position = Vector3.Lerp(this.transform.position, target.position + offset, Time.deltaTime * speed);
    }
}