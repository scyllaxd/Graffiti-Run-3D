using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

public class RotateObject : MonoBehaviour
{
    [BoxGroup("Rotate Options")]
    [OnValueChanged("OnValueChangedCallback")]
    public RotateOptions RotateState;
    [BoxGroup("Rotate Options")]
    public float RotateSpeed;
    private bool Left,Right;



    private void Start()
    {
        OnValueChangedCallback();
        
    }

    public void UpdateGameState(RotateOptions newState)
    {
        RotateState = newState;
        transform.eulerAngles = Vector3.zero;
        switch (newState)
        {
            case RotateOptions.Left:
                Left = true;
                Right = false;
            break;
            case RotateOptions.Right:;
                Left = false;
                Right = true;
            break;
        }
    }

    private void OnValueChangedCallback()
    {
        UpdateGameState(RotateState);
    }
    void Update()
    {
        if(Right)
            transform.Rotate(Vector3.up * (RotateSpeed * Time.deltaTime));
        if (Left)
            transform.Rotate(Vector3.down * (RotateSpeed * Time.deltaTime));
    }
}

public enum RotateOptions
{
    Left,
    Right
}
