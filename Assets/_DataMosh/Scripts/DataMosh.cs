using System;
using UnityEngine;

public class DataMosh : MonoBehaviour
{
    public Material DMMat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors;
    }

    private void Update()
    {
        Shader.SetGlobalInt("_Button", Input.GetButton("Fire1") ? 1 : 0);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, DMMat);
    }
}
