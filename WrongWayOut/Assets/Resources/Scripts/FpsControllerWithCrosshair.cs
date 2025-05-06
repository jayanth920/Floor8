using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FpsControllerWithCrosshair : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 2.0f;
    public Transform cameraTransform;
    float xRotation = 0f;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    private CharacterController controller;

    [Header("Interaction")]
    public float interactionRange = 5f;
    public LayerMask interactableLayer;
    public Image crosshairImage;
    public Button3d button3d;

    [Header("Camera Breathing")]
    public float idleBobFrequency = 1.5f;
    public float idleBobAmplitude = 0.03f;
    public float runBobFrequency = 4f;
    public float runBobAmplitude = 0.05f;
    private Vector3 initialCameraLocalPos;
    private float bobTimer = 0f;

    private void Start()
    {
        initialCameraLocalPos = cameraTransform.localPosition;
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (crosshairImage == null)
        {
            crosshairImage = FindFirstObjectByType<Image>();
            if (crosshairImage == null)
                Debug.LogWarning("Crosshair image not assigned.");
        }

        interactableLayer = LayerMask.GetMask("Interactable");
        if (crosshairImage != null) crosshairImage.enabled = false;
    }

    private void Update()
    {
        LookAround();
        Move();
        RaycastInteract();
        ApplyCameraBreathing();
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);
        float speed = isRunning ? runSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);
    }

    private void RaycastInteract()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * interactionRange, Color.green);

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            if (crosshairImage != null) crosshairImage.enabled = true;

            if (Input.GetMouseButtonDown(0) && button3d.clicking)  
            {
                Button3d button = hit.transform.GetComponent<Button3d>();
                if (button != null)
                {
                    button.ClickButton();  
                }
            }
        }
        else
        {
            if (crosshairImage != null) crosshairImage.enabled = false;
        }
    }

    private void ApplyCameraBreathing()
    {
        Vector3 velocity = controller.velocity;
        bool isMoving = velocity.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);

        if (isRunning)
        {
            // triangle wave 
            bobTimer += Time.deltaTime * runBobFrequency;
            float sharpBob = Mathf.PingPong(bobTimer, 1f) * 2f - 1f; // Triangle wave from -1 to 1
            float verticalOffset = sharpBob * runBobAmplitude;
            cameraTransform.localPosition = initialCameraLocalPos + new Vector3(0f, verticalOffset, 0f);
        }
        else if (isMoving)
        {
            // sin bob while walking
            bobTimer += Time.deltaTime * idleBobFrequency;
            float bobOffset = Mathf.Sin(bobTimer * 2f) * idleBobAmplitude;
            cameraTransform.localPosition = initialCameraLocalPos + new Vector3(0f, bobOffset, 0f);
        }
        else
        {
            // reset cam idle
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, initialCameraLocalPos, Time.deltaTime * 5f);
        }
    }
}
