using System;
using UnityEngine;
using UnityEngine.UI;

public class SDataMoshEffect : MonoBehaviour
{
    public Material DMMat;
    private RenderTexture _buffer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //Generate motion texture on main camera
        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors;
        _buffer = new RenderTexture(Screen.width, Screen.height, 16);
        
        //Debug: Display frame before effect is applied
        GameObject.FindGameObjectWithTag("RenderTexture").GetComponent<RawImage>().texture = _buffer;
    }

    private void Update()
    {
        //Set lerp value in shader to toggle effect
        if (Input.GetButton("Fire1"))
        {
            Shader.SetGlobalInteger("_Trigger", 1);
        }
        else
        {
            Shader.SetGlobalInteger("_Trigger", 0);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!_buffer)
        {
            _buffer = new RenderTexture(Screen.width, Screen.height, 16);
        }
        
        //Send previous frame to shader
        Shader.SetGlobalTexture("_Prev", _buffer);
        //Run shader on current frame
        Graphics.Blit(source, destination, DMMat);
        //Store output into buffer to use as previous frame in next iteration
        //Active render texture is null so it renders directly to main window.
        RenderTexture.active = Camera.main.targetTexture;
        Graphics.Blit(RenderTexture.active, _buffer);
        
        //Debug: Show current rendered frame before effect is applied
        GameObject.FindGameObjectWithTag("RenderTexture").GetComponent<RawImage>().texture = RenderTexture.active;
    }
}