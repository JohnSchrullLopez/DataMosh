using System;
using UnityEngine;
using UnityEngine.UI;

public class SDataMoshEffect : MonoBehaviour
{
    public Material DMMat;
    private RenderTexture _buffer;
    private int _triggerID;
    private int _prevID;
    [SerializeField] private int _BlockSize = 16;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        //Generate motion texture on main camera
        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors;
        _buffer = new RenderTexture(Screen.width, Screen.height, 16);
        
        //Cache property IDs and initialize
        _triggerID = Shader.PropertyToID("_Trigger");    
        _prevID = Shader.PropertyToID("_Prev");
        Shader.SetGlobalInteger("_BlockSize", _BlockSize);
    }

    private void Update()
    {
        //Set lerp value in shader to toggle effect
        if (Input.GetButton("Fire1"))
        {
            Shader.SetGlobalInteger(_triggerID, 1);
        }
        else
        {
            Shader.SetGlobalInteger(_triggerID, 0);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!_buffer)
        {
            _buffer = new RenderTexture(Screen.width, Screen.height, 16);
        }
        
        //Send previous frame to shader
        Shader.SetGlobalTexture(_prevID, _buffer);
        //Run shader on current frame
        Graphics.Blit(source, destination, DMMat);
        //Store output into buffer to use as previous frame in next iteration
        //Active render texture is null so it renders directly to main window.
        RenderTexture.active = Camera.main.targetTexture;
        Graphics.Blit(RenderTexture.active, _buffer);
    }
}