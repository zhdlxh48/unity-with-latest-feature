using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputControl : MonoBehaviour
{
    public delegate void InputButtonDelegate(bool state);

    public delegate void InputVector2Delegate(Vector2 value);

    public enum MouseButtonState
    {
        Press = 0,
        Release
    }

    private MouseInputAction.MouseActionMapActions _mouseActionMapActions;

    private MouseInputAction _mouseInputAction;

    public Vector2 MousePosition { get; private set; }
    public Vector2 MouseDelta { get; private set; }
    public Vector2 MouseScroll { get; private set; }
    public MouseButtonState LeftMouseButtonState { get; private set; }

    private void Awake()
    {
        _mouseInputAction = new MouseInputAction();
        _mouseActionMapActions = _mouseInputAction.MouseActionMap;

        LeftMouseButtonState = MouseButtonState.Press;
    }

    private void OnEnable()
    {
        _mouseInputAction.Enable();

        _mouseActionMapActions.MousePosition.performed += MousePositionListener;
        _mouseActionMapActions.MouseDelta.performed += MouseDeltaListener;
        _mouseActionMapActions.MouseScroll.performed += MouseScrollListener;
        _mouseActionMapActions.MouseLeft.performed += MouseLeftListener;
    }

    private void OnDisable()
    {
        _mouseInputAction.Disable();

        _mouseActionMapActions.MousePosition.performed -= MousePositionListener;
        _mouseActionMapActions.MouseDelta.performed -= MouseDeltaListener;
        _mouseActionMapActions.MouseScroll.performed -= MouseScrollListener;
        _mouseActionMapActions.MouseLeft.performed -= MouseLeftListener;
    }

    public event InputVector2Delegate OnMousePosition;
    public event InputVector2Delegate OnMouseDelta;
    public event InputVector2Delegate OnMouseScroll;
    public event InputButtonDelegate OnLeftButtonChanged;

    private void MousePositionListener(InputAction.CallbackContext obj)
    {
        MousePosition = obj.ReadValue<Vector2>();
        OnMousePosition?.Invoke(MousePosition);
    }

    private void MouseDeltaListener(InputAction.CallbackContext obj)
    {
        MouseDelta = obj.ReadValue<Vector2>();
        OnMouseDelta?.Invoke(MouseDelta);
    }

    private void MouseScrollListener(InputAction.CallbackContext obj)
    {
        MouseScroll = obj.ReadValue<Vector2>();
        OnMouseScroll?.Invoke(MouseScroll);
    }

    private void MouseLeftListener(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValueAsButton();
        LeftMouseButtonState = value ? MouseButtonState.Press : MouseButtonState.Release;
        OnLeftButtonChanged?.Invoke(value);
    }
}