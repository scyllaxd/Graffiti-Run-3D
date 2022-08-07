using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mpb : MonoBehaviour
{
    // Start is called before the first frame update
    public MeshRenderer meshRenderer;
    public MaterialPropertyBlock _propertyBlock;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        _propertyBlock = new MaterialPropertyBlock();

        meshRenderer.GetPropertyBlock(_propertyBlock); // Get previously set values. They will reset otherwise
        _propertyBlock.SetColor("_BaseColor", Color.white);
        meshRenderer.SetPropertyBlock(_propertyBlock, 0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
