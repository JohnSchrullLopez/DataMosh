using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SPlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _mouseSensitivity;
    
    private Camera _mainCamera;
    private Transform _playerTransform;
    private CharacterController _characterController;
    private SPlayerInput _playerInput;

    private Vector3 _movementInput;
    private Vector2 _camInput;
    private float _gravity = -9.81f;
    private float _yVelocity;
    private Vector2 _currentCameraRotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _mainCamera = GetComponentInChildren<Camera>();
        _playerInput = GetComponent<SPlayerInput>();
        _playerTransform = GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        GetCameraInput();
    }

    private void GetCameraInput()
    {
        //Get mouse movement each frame and add result to the transform's rotation
        _camInput = new Vector2(Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime, Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime);
        _currentCameraRotation += _camInput;
        
        //Horizontal cam movement
        transform.localRotation = Quaternion.AngleAxis(_currentCameraRotation.x, Vector3.up);
        //Vertical cam movement
        _mainCamera.transform.localRotation = Quaternion.AngleAxis(-_currentCameraRotation.y, Vector3.right);
    }

    private void UpdateMovement()
    {
        //Gravity
        if (_characterController.isGrounded && _yVelocity < 0)
        {
            _yVelocity = 0f;
        }
        _yVelocity += _gravity * Time.deltaTime;

        //WASD movement
        /*_movementInput = new Vector3(Input.GetAxis("Horizontal"), _yVelocity, Input.GetAxis("Vertical"));
        _movementInput = _playerTransform.TransformDirection(_movementInput);*/

        _movementInput = new Vector3(_playerInput.MoveInput.x, _yVelocity, _playerInput.MoveInput.y);
        _movementInput = _playerTransform.TransformDirection(_movementInput);
        _characterController.Move(_movementInput * _moveSpeed * Time.deltaTime);
    }
}
