using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _lookSpeed;
    [SerializeField] private Camera _camera;

    private PlayerInput _inputActions;

    private Vector2 _moveDirection;
    private Vector2 _lookDirection;

    public event Action<Vector3> Clicked;

    private void Awake()
    {
        _inputActions = new PlayerInput();

        _inputActions.Player.Click.performed += OnClick;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();

    }

    private void Update()
    {
        _moveDirection = _inputActions.Player.Move.ReadValue<Vector2>();
        _lookDirection = _inputActions.Player.Look.ReadValue<Vector2>();

        Move();
        Look();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 tapPoint = context.ReadValue<Vector2>();

            Ray castPoint = Camera.main.ScreenPointToRay(tapPoint);
            RaycastHit hit;

            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                Clicked?.Invoke(hit.point);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.action.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookDirection = context.action.ReadValue<Vector2>();
    }

    private void Move()
    {
        if (_moveDirection.sqrMagnitude < 0.1f)
            return;

        float scaledMoveSpeed = _moveSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(_moveDirection.x, 0f, _moveDirection.y) * scaledMoveSpeed;

        transform.Translate(offset);
    }

    private void Look()
    {
        if (_lookDirection.sqrMagnitude < 0.1f)
            return;

        float scaledLookSpeed = _lookSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(-_lookDirection.y, _lookDirection.x, 0f) * scaledLookSpeed;

        transform.Rotate(offset);
    }
}
