using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveObjectsUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    transform.DOMoveY(transform.position.y + 0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
