using UnityEngine;
using UnityEngine.InputSystem;

public class SPlayerInput : MonoBehaviour
{
    private FPInput _inputActions;

    public Vector2 MoveInput;

    private InputAction _MovementAction;

    private void Awake()
    {
        _inputActions = new FPInput();
        _inputActions.Enable();

        _MovementAction = _inputActions.FindAction("WASD Movement");
    }

    private void Update()
    {
        MoveInput = _MovementAction.ReadValue<Vector2>();
    }
}
