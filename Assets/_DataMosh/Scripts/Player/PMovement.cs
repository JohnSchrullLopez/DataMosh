using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _mouseSensitivity;
    private Rigidbody _rigidbody;
    private Vector3 _moveAxis;
    private Vector2 _camMoveAxis;
    private Camera _mainCamera;
    private Transform _playerTransform;
    private float xMouse, yMouse, xRotation, yRotation, xAxis, zAxis;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _moveAxis = Vector3.zero;
        _camMoveAxis = Vector3.zero;
        _playerTransform = GetComponent<Transform>();
        _mainCamera = GetComponentInChildren<Camera>();
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        GetCameraInput();
        
        transform.rotation = Quaternion.Euler(0, xRotation, 0);
        _playerTransform.Rotate(Vector3.up * xMouse);
        _mainCamera.transform.Rotate(Vector3.left * -yMouse);
    }

    private void GetCameraInput()
    {
        xMouse = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        yMouse = -Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        yRotation += yMouse;
        xRotation += xMouse;
        yRotation = Mathf.Clamp(yRotation, -90, 90);
    }

    private void GetMovementInput()
    {
        xAxis = Input.GetAxis("Horizontal") * _moveSpeed * Time.deltaTime;
        zAxis = Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * zAxis + transform.up * _rigidbody.velocity.y + transform.right * xAxis;
    }
}
