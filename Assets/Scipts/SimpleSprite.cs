using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SimpleSprite : MonoBehaviour, IDestroyOnClone
{
    public Texture2D texture;
    public MeshRenderer mr;
    [ColorUsage(true, true)] public Color color = Color.white;

    public MaterialPropertyBlock _materialPropertyBlock;


    void OnEnable()
    {
        mr = GetComponent<MeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        if (texture != null)
        {
            _materialPropertyBlock.SetTexture("_MainTex", texture);
            _materialPropertyBlock.SetColor("_TintColor", color); 
        }

        mr.SetPropertyBlock(_materialPropertyBlock);
    }

    private void OnValidate()
    {
        OnEnable();
    }

    public void SetColor(Color color)
    {
        this.color = color;
        _materialPropertyBlock?.SetColor("_TintColor", color);
        mr.SetPropertyBlock(_materialPropertyBlock);
    }
}