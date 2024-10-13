using UnityEngine;
using UnityEngine.InputSystem;

public class SPlayerInput : MonoBehaviour
{
    private FPInput _inputActions;

    public Vector2 MoveInput;
    public bool _jumpPressed = false;

    private InputAction _MovementAction;
    private InputAction _JumpAction;

    private void Awake()
    {
        _inputActions = new FPInput();
        _inputActions.Enable();

        _MovementAction = _inputActions.FindAction("WASD Movement");
        _JumpAction = _inputActions.FindAction("Jump");

        _JumpAction.started += (context =>
        {
            _jumpPressed = true;
        });
    }

    private void Update()
    {
        MoveInput = _MovementAction.ReadValue<Vector2>();
    }
}
