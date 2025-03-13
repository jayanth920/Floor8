using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro

public class PlayerNew : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI instructionText; // Assign this in the Inspector

    [Header("Movement")]
    public float playerSpeed = 10.0f;
    public float gravity = -9.8f;
    CharacterController characterController;

    [Header("Key Handling")]
    private bool hasKey = false;
    public Transform keyHolder;
    private GameObject nearbyKey;
    private GameObject heldKey;
    private float keyTimer = 0f;
    private float keyTimeLimit = 5f; // 5 seconds to choose a door

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
        UpdateInstructionText("Find the Key");
    }

    void Update()
    {
        ApplyGravityWithCC();
        MouseLook();

        if (nearbyKey != null && Input.GetKeyDown(KeyCode.E) && !hasKey)
        {
            PickupKey();
        }

        if (hasKey)
        {
            keyTimer += Time.deltaTime;
            if (keyTimer >= keyTimeLimit)
            {
                RestartGame(); // If time runs out, restart
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key") && !hasKey)
        {
            nearbyKey = other.gameObject;
            UpdateInstructionText("Press E to Pick Up");
        }
        else if ((other.gameObject.CompareTag("FrontDoor") || other.gameObject.CompareTag("BackDoor")) && hasKey)
        {
            RestartGame(); // Restart when colliding with a door while holding the key
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Key") && nearbyKey == other.gameObject)
        {
            nearbyKey = null;
            UpdateInstructionText("Find the Key");
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
        keyTimer = 0f; // Start the timer
        UpdateInstructionText("Choose a Door!");
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateInstructionText(string newText)
    {
        if (instructionText != null)
        {
            instructionText.text = newText;
        }
    }

    Vector3 gravityVelocity = Vector3.zero;

    void ApplyGravityWithCC()
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