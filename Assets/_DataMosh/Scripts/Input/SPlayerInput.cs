using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SPlayerInput : MonoBehaviour
{
    private FPInput _inputActions;

    public Vector2 MoveInput;
    public bool JumpPressed = false;
    public bool Sliding = false;
    public bool SlideStarted = false;

    private InputAction _movementAction;
    private InputAction _jumpAction;
    private InputAction _slideAction;

    private void Awake()
    {
        _inputActions = new FPInput();
        _inputActions.Enable();

        _movementAction = _inputActions.FindAction("WASD Movement");
        _jumpAction = _inputActions.FindAction("Jump");
        _slideAction = _inputActions.FindAction("Slide");

        _jumpAction.started += (context =>
        {
            JumpPressed = true;
        });

        _slideAction.started += (context =>
        {
            Sliding = true;
            SlideStarted = true;
        });

        _slideAction.canceled += (context =>
        {
            Sliding = false;
        });
    }

    private void Update()
    {
        MoveInput = _movementAction.ReadValue<Vector2>();
    }
}
