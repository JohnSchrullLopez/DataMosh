using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SPlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _sensitivity = 1.0f;
    [SerializeField] private float _movementSpeed = 1.0f;
    [Header("Player Physics")]
    [SerializeField] private float _groundDrag = 5f;
    [SerializeField] private float _airDrag = 2f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _gravity = 9.81f;

    private SPlayerInput _input;
    private Transform _orientation;
    private Transform _camera;
    private Vector3 _movementDirection;
    private Rigidbody _playerRB;
    public bool _isGrounded = false;
    private float yRotation = 0.0f;
    private float xRotation = 0.0f;
    private float _playerHeight = 2f;

    private void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _playerRB = GetComponent<Rigidbody>();
        _input = GetComponent<SPlayerInput>();
        _orientation = transform.GetChild(0).GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MoveCamera();
        ApplyDrag();
        Jump();

        Physics.gravity = new Vector3(0, -_gravity, 0);
    }

    private void Jump()
    {
        if (_input._jumpPressed && _isGrounded)
        {
            _playerRB.AddForce(_orientation.up * _jumpForce, ForceMode.Impulse);
            _input._jumpPressed = false;
        }
    }

    private void ApplyDrag()
    {
        GroundCheck();

        if (_isGrounded)
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

        _playerRB.AddForce(_movementDirection * _movementSpeed, ForceMode.Force);
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
