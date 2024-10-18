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
    public bool Dashing = false;

    private InputAction _movementAction;
    private InputAction _jumpAction;
    private InputAction _slideAction;
    private InputAction _dashAction;

    private void Awake()
    {
        _inputActions = new FPInput();
        _inputActions.Enable();

        _movementAction = _inputActions.FindAction("WASD Movement");
        _jumpAction = _inputActions.FindAction("Jump");
        _slideAction = _inputActions.FindAction("Slide");
        _dashAction = _inputActions.FindAction("Dash");

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

        _dashAction.started += (context =>
        {
            Dashing = true;
        });
    }

    private void Update()
    {
        MoveInput = _movementAction.ReadValue<Vector2>();
    }

    public void SetSlideStarted(bool started)
    {
        SlideStarted = started;
    }

    public void SetSliding(bool sliding)
    {
        Sliding = sliding;
    }

    public void SetDashing(bool dashing)
    {
        Dashing = dashing;
    }
}
