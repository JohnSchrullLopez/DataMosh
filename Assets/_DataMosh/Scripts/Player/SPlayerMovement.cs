using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

//TODO: Add cooldown to dash

public class SPlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _sensitivity = 1.0f;
    [SerializeField] private float _movementSpeed = 1.0f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _slideBoost = 2f;
    [SerializeField, Range(0, 1)] private float _airControl = .2f;
    [SerializeField] private float _dashForce = 1f;
    [SerializeField] private float _maxSpeed = 25f;
    [SerializeField] private float _wallJumpHeight = 20f;
    
    [Header("Player Physics")]
    [SerializeField] private float _groundDrag = 5f;
    [SerializeField] private float _airDrag = 2f;
    [SerializeField] private float _slideDrag = 0.1f;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _wallLayerMask;
    [SerializeField] private float _gravity = 9.81f;

    private SPlayerInput _input;
    private Transform _orientation;
    private AttachCamera _camFollow;
    private Transform _camera;
    private Vector3 _movementDirection;
    private Rigidbody _playerRB;
    public bool _isGrounded = false;
    private float yRotation = 0.0f;
    private float xRotation = 0.0f;
    private float _playerHeight = 2f;

    //Wall Running
    [SerializeField] float _wallRunJumpForce = 15f;
    private bool _wallRunning = false;
    private bool _wallLeft = false;
    private RaycastHit _wallLeftHit;
    private bool _wallRight = false;
    private RaycastHit _wallRightHit;
    private float _wallRunSpeed;
    private float _wallRunCooldownCurrent = 0f;
    private float _wallRunCooldownMax = .15f;

    private void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _playerRB = GetComponent<Rigidbody>();
        _input = GetComponent<SPlayerInput>();
        _orientation = transform.GetChild(0).GetComponent<Transform>();
        _camFollow = GameObject.Find("CamParent").GetComponent<AttachCamera>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MoveCamera();
        ApplyDrag();
        Jump();
        HandleCooldowns();
        CapSpeed();
        
        //Debug.Log(_playerRB.linearVelocity.magnitude);
        Physics.gravity = new Vector3(0, -_gravity, 0);
    }

    private void CapSpeed()
    {
        if (_playerRB.linearVelocity.magnitude > _maxSpeed && !_input.Sliding)
        {
            Debug.Log("Capping");
            _playerRB.AddForce(-_playerRB.linearVelocity, ForceMode.Force);
        }
    }

    private void HandleCooldowns()
    {
        if (_wallRunCooldownCurrent > 0) { _wallRunCooldownCurrent -= Time.deltaTime; }
    }

    private void Jump()
    {
        if (_input.JumpPressed && _isGrounded)
        {
            _playerRB.AddForce(_orientation.up * _jumpForce, ForceMode.Impulse);
            _input.JumpPressed = false;
        }
    }

    private void ApplyDrag()
    {
        GroundCheck();

        if (_input.Sliding && _isGrounded)
        {
            _playerRB.linearDamping = _slideDrag;
        }
        else if (_isGrounded)
        {
            _playerRB.linearDamping = _groundDrag;
        }
        else
        {
            _playerRB.linearDamping = _airDrag;
        }
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.1f, _groundLayerMask);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        _movementDirection = _orientation.forward * _input.MoveInput.y + _orientation.right * _input.MoveInput.x;

        CheckForWalls();

        if (_wallLeft || _wallRight)
        {
            WallRun();
            _wallRunning = true;
            return;
        }
        else
        {
            _playerRB.useGravity = true;
            _wallRunning = false;
        }

        //Handle slide
        if (_input.Sliding && _isGrounded)
        {
            Slide();
            return;
        }
        
        if (_isGrounded) //Handle ground movement
        {
            _playerRB.AddForce(_movementDirection * _movementSpeed, ForceMode.Force);
        }
        else //Handles air movement
        {
            //Nullifies slide input if player is in air
            _input.SetSlideStarted(false);
            _input.SetSliding(false);

            _playerRB.AddForce(_movementDirection * (_movementSpeed * _airControl), ForceMode.Force);
        }

        //Dashing
        Dash();

        //Reset cam position when not sliding
        _camFollow.MoveToDefaultPosition();
    }

    private void CheckForWalls()
    {
        if (_wallRunCooldownCurrent <= 0)
        {
            _wallLeft = Physics.Raycast(transform.position, -_orientation.right, out _wallLeftHit, 1f, _wallLayerMask);
            _wallRight = Physics.Raycast(transform.position, _orientation.right, out _wallRightHit, 1f, _wallLayerMask);
        }
    }

    private void WallRun()
    {
        //Set wall run velocity if wall run has just started
        if (!_wallRunning)
        {
            _wallRunSpeed = _playerRB.linearVelocity.magnitude; // 1.5f;
        }

        _playerRB.useGravity = false;

        if (_wallLeft)
        {
            _playerRB.linearVelocity = Vector3.Cross(-_orientation.up, _wallLeftHit.normal) * _wallRunSpeed;
            _playerRB.AddForce(-_orientation.right * 10 * Time.deltaTime);
            if (_input.JumpPressed) 
            {
                _wallRunCooldownCurrent = _wallRunCooldownMax;
                WallRunJump(_wallLeftHit); 
            }
        }
        else
        {
            _playerRB.linearVelocity = Vector3.Cross(_orientation.up, _wallRightHit.normal) * _wallRunSpeed;
            _playerRB.AddForce(_orientation.right * 10 * Time.deltaTime);
            if (_input.JumpPressed) 
            { 
                _wallRunCooldownCurrent = _wallRunCooldownMax;
                WallRunJump(_wallRightHit);
            }
        }
    }

    private void WallRunJump(RaycastHit wall)
    {
        Vector3 jumpVector = (wall.normal + _playerRB.linearVelocity / 4f).normalized * _wallRunJumpForce;
        jumpVector += new Vector3(0, _wallJumpHeight, 0);
        _playerRB.AddForce(jumpVector, ForceMode.Impulse);
        _input.JumpPressed = false;
        _wallLeft = false;
        _wallRight = false;
    }

    private void Slide()
    {
        if (_input.SlideStarted)
        {
            _playerRB.linearVelocity = _playerRB.linearVelocity * _slideBoost;
            _input.SlideStarted = false;
            _camFollow.MoveToSlidePosition();
        }
    }

    private void Dash()
    {
        if(_input.Dashing)
        {
            _playerRB.linearVelocity = Vector3.zero;
            _playerRB.AddForce(_camera.forward * _dashForce, ForceMode.Impulse);
            _input.SetDashing(false);
        }
    }

    private void MoveCamera()
    {
        yRotation += Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensitivity;
        xRotation -= Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        _camera.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        _orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }



    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - (_playerHeight * 0.5f + 0.2f), transform.position.z), Color.red, 1f);
    }
}
