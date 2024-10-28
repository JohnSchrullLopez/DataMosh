using System;
using UnityEngine;
using UnityEngine.UI;

public class SDataMoshEffect : MonoBehaviour
{
    //TODO: Organize Code, Fix Object mask occlusion bug
    public Material DMMat;
    public RawImage DebugImage;
    private RenderTexture _objectMask;
    private RenderTexture _topMask;
    private RenderTexture _buffer;
    private int _intensityID;
    private int _prevID;
    [SerializeField] private int _BlockSize = 16;
    [SerializeField, Range(0.7f, 1.9f)] private float _PerBlockNoise = 1.4f;
    [SerializeField, Range(0.7f, 1.9f)] private float _BlockDecay = 1.4f;
    [SerializeField, Range(0, 1)] public float _intensityValue = 0.0f;
    private bool _transitioningInto = false;
    private float lerpVal = 0.0f;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        //Generate motion texture on main camera
        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors;
        _buffer = new RenderTexture(Screen.width, Screen.height, 16);
        _objectMask = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 16);
        _topMask = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 16);

        //Cache property IDs and initialize
        _intensityID = Shader.PropertyToID("_DMIntensity");
        _prevID = Shader.PropertyToID("_Prev");
        Shader.SetGlobalInteger("_BlockSize", _BlockSize);
        Shader.SetGlobalFloat("_PerBlockNoise", _PerBlockNoise);
        Shader.SetGlobalFloat("_BlockDecay", _BlockDecay);

        //Debugging
        transform.GetChild(0).GetComponent<Camera>().targetTexture = _objectMask;
        transform.GetChild(1).GetComponent<Camera>().targetTexture = _topMask;
        DebugImage.texture = Camera.main.targetTexture;
    }

    private void Update()
    {
        //Set lerp value in shader to toggle effect
        if (Input.GetMouseButtonDown(1))
        {
            _transitioningInto = !_transitioningInto;
        }

        SmoothTransition();
        Shader.SetGlobalFloat(_intensityID, _intensityValue);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!_buffer)
        {
            _buffer = new RenderTexture(Screen.width, Screen.height, 16);
        }

        //Send previous frame and object mask to shader
        Shader.SetGlobalTexture(_prevID, _buffer);
        Shader.SetGlobalTexture("_Mask", _objectMask);
        Shader.SetGlobalTexture("_Top", _topMask);

        //Run shader on current frame
        Graphics.Blit(source, destination, DMMat);

        //Store output into buffer to use as previous frame in next iteration
        //Active render texture is null so it renders directly to main window.
        RenderTexture.active = Camera.main.targetTexture;
        Graphics.Blit(RenderTexture.active, _buffer);
    }

    private void SmoothTransition()
    {
        if (_transitioningInto)
        {
            //DOTween.To(() => _intensityValue, x => _intensityValue = x, 1f, 0.5f);
            lerpVal += Time.deltaTime * 0.5f;
        }
        else
        {
            //DOTween.To(() => _intensityValue, x => _intensityValue = x, 0f, 5);
            lerpVal -= Time.deltaTime * 0.5f;
        }

        lerpVal = Mathf.Clamp(lerpVal, 0.0f, 1.0f);
        _intensityValue = Mathf.Lerp(0f, 1f, lerpVal);
    }
}