using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InterfaceAnimator : MonoBehaviour
{

    public GameObject swipeToPlay;
    public GameObject amazing;
    public GameObject fail;

    // Start is called before the first frame update
    void Start()
    {
        swipeToPlay.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        amazing.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        fail.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
