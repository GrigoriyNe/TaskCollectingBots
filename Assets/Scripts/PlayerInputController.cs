using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _lookSpeed;
    [SerializeField] private Camera _camera;

    private PlayerInput _input;

    private Vector2 _moveDirection;
    private Vector2 _lookDirection;
    private Vector2 _tapPoint;

    public event Action<RaycastHit> Clicked;

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Click.started += OnClick;
        _input.Player.Click.performed += OnClick;
        _input.Player.Click.canceled += OnClick;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Click.started -= OnClick;
        _input.Player.Click.performed -= OnClick;
        _input.Player.Click.canceled -= OnClick;
    }

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void Update()
    {
        _moveDirection = _input.Player.Move.ReadValue<Vector2>();
        _lookDirection = _input.Player.Look.ReadValue<Vector2>();

        Move();
        Look();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _tapPoint = Vector2.zero;
        }
        else if (context.performed)
        {
            context.ReadValue<Vector2>();
            _tapPoint = context.ReadValue<Vector2>();

            if (_tapPoint != Vector2.zero)
            {
                Ray castPoint = Camera.main.ScreenPointToRay(_tapPoint);
                RaycastHit hit;

                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                {
                    Clicked?.Invoke(hit);
                    context = new InputAction.CallbackContext();
                }
            }
        }
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
