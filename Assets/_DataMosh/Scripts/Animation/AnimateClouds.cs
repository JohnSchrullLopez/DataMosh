using System;
using UnityEngine;

public class AnimateClouds : MonoBehaviour
{
    [SerializeField, Range(0, .01f)] private float animationSpeed = 0.2f;
    private Material _cloudMat;
    private float value = 0.0f;
    
    private void Awake()
    {
        _cloudMat = GetComponent<MeshRenderer>().material;
        value = 0.0f;
    }

    private void Update()
    {
        value += Time.deltaTime * animationSpeed;
        _cloudMat.SetTextureOffset("_MainTex", new Vector2(value, 0));
    }
}
