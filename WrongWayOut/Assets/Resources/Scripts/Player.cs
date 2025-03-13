using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro

public class Player : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI instructionText; // Assign this in the Inspector

    [Header("Movement")]
    public float playerSpeed = 10.0f;
    public float gravity = -9.8f;
    CharacterController characterController;

    [Header("Key & Door Handling")]
    private bool hasKey = false;
    public Transform keyHolder;
    private GameObject nearbyKey;
    private GameObject heldKey;

    [Header("Flashlight Handling")]
    public GameObject flashlight; // Drag flashlight object here
    private bool isFlashlightOn = false;

    [Header("Mouse Look Settings")]
    public float mouseSensitivityX = 2.0f;
    public float mouseSensitivityY = 2.0f;
    public Transform playerBody;

    private float xRotation = 0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize instruction text
        if (instructionText != null)
        {
            instructionText.text = "F to Toggle Flashlight";
        }


    }

    void Update()
    {
        ApplyGravityWithCC();
        MouseLook();

        // Handle key pickup
        if (nearbyKey != null && Input.GetKeyDown(KeyCode.E) && !hasKey)
        {
            PickupKey();
        }

        // Handle flashlight toggle
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key") && !hasKey)
        {
            nearbyKey = other.gameObject;
            UpdateInstructionText("Press E to Pick Up | F to Toggle Flashlight");
        }
        if (other.gameObject.CompareTag("Door31") && hasKey)
        {
            RotateAndMoveDoor(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Key") && nearbyKey == other.gameObject)
        {
            nearbyKey = null;
            UpdateInstructionText("F to Toggle Flashlight");
        }
    }

    void PickupKey()
    {
        hasKey = true;
        heldKey = nearbyKey;

        heldKey.transform.SetParent(keyHolder);
        heldKey.transform.localPosition = Vector3.zero;
        heldKey.transform.localRotation = Quaternion.identity;

        nearbyKey = null;
        UpdateInstructionText("F to Toggle Flashlight");
    }

    void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.SetActive(isFlashlightOn);
        }
    }

    void RotateAndMoveDoor(GameObject door)
    {
        door.transform.position = new Vector3(door.transform.position.x, 16.5f, 19.2f);
        door.transform.rotation = Quaternion.Euler(0, 90, 0);

        Destroy(heldKey);
        hasKey = false;
    }

    void UpdateInstructionText(string newText)
    {
        if (instructionText != null)
        {
            instructionText.text = newText;
        }
    }

    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    Vector3 gravityVelocity = Vector3.zero;

    public void ApplyGravityWithCC()
    {
        if (characterController.isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity = Vector3.zero;
            return;
        }

        gravityVelocity.y += gravity * Time.deltaTime;
        characterController.Move(gravityVelocity * Time.deltaTime);
    }

    public void MoveWithCC(Vector3 direction)
    {
        characterController.Move(direction * playerSpeed * Time.deltaTime);
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}