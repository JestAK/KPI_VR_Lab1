using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float defaultSpeed = 5f;
    public float sprintSpeed = 10f;

    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;

    public float mouseSensitivity = 0.5f;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    private CharacterController controller;
    private Camera playerCamera;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private float cameraPitch = 0f;

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }

        MovePlayer();
    }

    private void LookAround()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        transform.Rotate(0f, mouseX, 0f);

        cameraPitch -= mouseY;

        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);

        playerCamera.transform.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void MovePlayer()
    {
        groundedPlayer = controller.isGrounded;

        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movementDirection = forward * input.y + right * input.x;

        movementDirection = Vector3.ClampMagnitude(movementDirection, 1f);

        float speed = Keyboard.current.shiftKey.isPressed
            ? sprintSpeed
            : defaultSpeed;

        if (groundedPlayer && jumpAction.action.WasPressedThisFrame())
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        Vector3 finalMove = movementDirection * speed + Vector3.up * playerVelocity.y;

        controller.Move(finalMove * Time.deltaTime);
    }
}