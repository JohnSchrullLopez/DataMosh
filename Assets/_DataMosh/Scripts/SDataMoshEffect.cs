using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SDataMoshEffect : MonoBehaviour
{
    private STargetController _targetController;
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
    private float lerpVelocity;
    private bool _renderingPaused = false;
    private bool _falling = false;
    private bool _respawning = false;
    private bool _debugging = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _targetController = GameObject.FindGameObjectWithTag("Target Controller").GetComponent<STargetController>();
    }

    private void Start()
    {
        //Generate motion texture on main camera
        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors;
        
        //Create textures
        _buffer = new RenderTexture(Screen.width, Screen.height, 16);
        _objectMask = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 16);
        _topMask = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 16);

        //Cache property IDs and initialize
        _intensityID = Shader.PropertyToID("_DMIntensity");
        _prevID = Shader.PropertyToID("_Prev");
        Shader.SetGlobalInteger("_BlockSize", _BlockSize);
        Shader.SetGlobalFloat("_PerBlockNoise", _PerBlockNoise);
        Shader.SetGlobalFloat("_BlockDecay", _BlockDecay);

        //Render cameras into mask textures
        transform.GetChild(0).GetComponent<Camera>().targetTexture = _objectMask;
        transform.GetChild(1).GetComponent<Camera>().targetTexture = _topMask;
        //DebugImage.texture = Camera.main.targetTexture;
    }

    private void Update()
    {
        EffectSmoothing();
        Shader.SetGlobalFloat(_intensityID, _intensityValue);

        if (Input.GetMouseButtonDown(1))
        {
            _debugging = !_debugging;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!_buffer)
        {
            _buffer = new RenderTexture(Screen.width, Screen.height, 16);
        }

        if (_renderingPaused)
        {
            RenderTexture.active = _buffer;
            return;
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

    private void EffectSmoothing()
    {
        if (_debugging)
        {
            _intensityValue = 1.0f;
            return;
        }
        
        if (_falling && !_respawning)
        {
            if (lerpVal >= 1)
            {
                RespawnWithEffect();
            }
            lerpVal += Time.deltaTime * 2f;
            lerpVal = Mathf.Clamp(lerpVal, 0, 1);
            _intensityValue = lerpVal;
        }
        else if (!_respawning)
        {
            float target = _targetController.GetDistanceToTarget();
            lerpVal = Mathf.Lerp(lerpVal, target, Time.deltaTime * 2.5f);
            _intensityValue = lerpVal;
        }
    }

    public void StartTransitionToEnd()
    {
        StartCoroutine(TransitionToEnd());
    }
    
    public IEnumerator TransitionToEnd()
    {
        _respawning = true;
        _renderingPaused = true;
        yield return new WaitForSecondsRealtime(0.5f);
        SGameManager.EndLevel();
        _renderingPaused = false;
        _respawning = false;
    }
    
    public void RespawnWithEffect()
    {
        StartCoroutine(RespawnPlayer());
    }
    
    public IEnumerator RespawnPlayer()
    {
        _respawning = true;
        _renderingPaused = true;
        SPlayerSpawnManager.Instance.RespawnPlayer();
        yield return new WaitForSecondsRealtime(0.5f);
        _renderingPaused = false;
        _respawning = false;
        _falling = false;
    }

    public void SetFalling(bool value)
    {
        _falling = value;
    }
}