using System;
using UnityEngine;

public class DataMosh : MonoBehaviour
{
    public Material DMMat;
    private RenderTexture _buffer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors;

        _buffer = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
    }

    private void Update()
    {
        /*if (Input.GetButton("Fire1"))
        {
            Shader.SetGlobalInteger("_Trigger", 1);
        }
        else
        {
            Shader.SetGlobalInteger("_Trigger", 0);
        }*/
        
        Shader.SetGlobalInteger("_Trigger", Input.GetButton("Fire1") ? 1 : 0);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Copy current frame into buffer texture
        Graphics.Blit(source, _buffer, DMMat);

        //After shader finishes with buffer copy the texture into camera output
        //Graphics.Blit(_buffer, destination);
        Graphics.Blit(_buffer, (RenderTexture)null);
    }
}