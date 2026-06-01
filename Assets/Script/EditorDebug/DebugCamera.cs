using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class DebugCamera : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float sprintMultiplier = 3f;
    [SerializeField] private float zoomSpeed = 10f;

    [Header("Rotation")]
    [SerializeField] private float rotationSensitivity = 3f;

    [Header("Pan")]
    [SerializeField] private float panSensitivity = 0.01f;

    [Header("Orbit")]
    [SerializeField] private float orbitSensitivity = 5f;

    private Vector3 orbitPoint;

    private void Start()
    {
        orbitPoint = transform.position + transform.forward * 10f;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
        HandlePan();
        HandleZoom();
        HandleOrbit();
    }

    private void HandleLook()
    {
        // RMB drag = Scene view rotate
        if (!Input.GetMouseButton(1))
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mouseX * rotationSensitivity, Space.World);
        transform.Rotate(Vector3.right, -mouseY * rotationSensitivity, Space.Self);
    }

    private void HandleMovement()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            move += transform.forward;

        if (Input.GetKey(KeyCode.S))
            move -= transform.forward;

        if (Input.GetKey(KeyCode.A))
            move -= transform.right;

        if (Input.GetKey(KeyCode.D))
            move += transform.right;

        if (Input.GetKey(KeyCode.Q))
            move -= transform.up;

        if (Input.GetKey(KeyCode.E))
            move += transform.up;

        float speed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed *= sprintMultiplier;

        transform.position += move * speed * Time.deltaTime;
    }

    private void HandlePan()
    {
        // MMB drag = pan
        if (!Input.GetMouseButton(2))
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 offset =
            (-transform.right * mouseX +
             -transform.up * mouseY)
            * panSensitivity * moveSpeed;

        transform.position += offset;
        orbitPoint += offset;
    }

    private void HandleZoom()
    {
        // Scroll = zoom
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) < 0.01f)
            return;

        transform.position +=
            transform.forward *
            scroll *
            zoomSpeed *
            Mathf.Max(moveSpeed, 1f) *
            Time.deltaTime;
    }

    private void HandleOrbit()
    {
        // Alt + LMB = Scene view orbit
        if (!(Input.GetKey(KeyCode.LeftAlt) &&
              Input.GetMouseButton(0)))
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.RotateAround(
            orbitPoint,
            Vector3.up,
            mouseX * orbitSensitivity);

        transform.RotateAround(
            orbitPoint,
            transform.right,
            -mouseY * orbitSensitivity);
    }
}