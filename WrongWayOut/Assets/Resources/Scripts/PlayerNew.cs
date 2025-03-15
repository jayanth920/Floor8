using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class PlayerNew : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI instructionText;
    // public TextMeshProUGUI roomLabel;
    public TextMeshPro roomNumberText; // Assign this in the Inspector

    public GameObject instructionPanel;

    [Header("Movement")]
    public float playerSpeed = 10.0f;
    public float gravity = -9.8f;
    CharacterController characterController;

    [Header("Key Handling")]
    private bool hasKey = false;
    public Transform keyHolder;
    public GameObject keyPrefab; // Assign in inspector
    private GameObject nearbyKey;
    private GameObject heldKey;
    private float keyTimer = 0f;
    private float keyTimeLimit = 5f;
    private Vector3 keySpawnPosition = new Vector3(7.4f, 1.1f, 0f); // Adjust as needed

    [Header("Room & Anomaly System")]
    private int roomNumber = 0;
    private bool hasAnomaly;
    private Vector3 respawnPosition = new Vector3(8f, 1f, -6.5f); // Respawn location

    [Header("Mouse Look Settings")]
    public float mouseSensitivityX = 2.0f;
    public float mouseSensitivityY = 2.0f;
    public Transform playerBody;

    [Header("Anomaly Prefabs")]
    private List<System.Action> anomalies = new List<System.Action>();
    private List<GameObject> activeAnomalies = new List<GameObject>();

    public GameObject plantPrefab; // Assign this in the Unity Inspector


    private float xRotation = 0f;
    private bool isPaused = false;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        instructionPanel.SetActive(false);

        SetUpNewRoom();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInstructions();
        }

        if (isPaused) return;

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
                Respawn(); // Reset room number if time runs out
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
            // Destroy the key when any door is touched (no matter if it's right or wrong)
            DestroyKey();

            // Check if the choice is correct and update room number
            CheckDoorChoice(other.gameObject.tag);
        }
    }

    void DestroyKey()
    {
        if (heldKey != null)
        {
            Destroy(heldKey);  // Destroy the key the player is holding
            heldKey = null;
        }
    }

    void CheckDoorChoice(string chosenDoor)
    {
        bool correctChoice = (hasAnomaly && chosenDoor == "BackDoor") || (!hasAnomaly && chosenDoor == "FrontDoor");

        if (correctChoice)
        {
            roomNumber++;  // Increment room number if the choice was correct

            if (roomNumber > 5) // If player completes room 5
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("Win"); // Load the win scene
                return;
            }
        }
        else
        {
            roomNumber = 0;  // Reset room number if the choice was incorrect
        }

        Respawn();  // Respawn the player and reinstantiate the key
    }


    void Respawn()
    {
        hasKey = false;
        keyTimer = 0f;

        characterController.enabled = false;
        transform.position = respawnPosition;  // Respawn the player to the initial position
        characterController.enabled = true;

        SetUpNewRoom();  // Ensure a new key is instantiated at the correct position
    }




    void SetUpNewRoom()
    {
        // Remove previous anomalies before setting up the new room
        ClearAnomalies();

        hasAnomaly = Random.value > 0.5f;
        Debug.Log("Anomaly: " + hasAnomaly);

        UpdateRoomLabel();
        UpdateInstructionText("Find the Key");

        // Add anomalies to the list
        anomalies.Clear();
        anomalies.Add(AddExtraPlant);

        // If anomaly is active, pick a random one
        if (hasAnomaly && anomalies.Count > 0)
        {
            int index = Random.Range(0, anomalies.Count);
            anomalies[index].Invoke();
        }

        // Instantiate the key at the correct position
        if (keyPrefab != null)
        {
            InstantiateKey();
        }
    }

    void ClearAnomalies()
    {
        foreach (GameObject anomaly in activeAnomalies)
        {
            Destroy(anomaly);
        }
        activeAnomalies.Clear();
    }




    void InstantiateKey()
    {
        if (heldKey != null) Destroy(heldKey);  // Prevent multiple keys if any exists
        heldKey = Instantiate(keyPrefab, keySpawnPosition, Quaternion.Euler(0, -45f, 0f));  // Instantiate the key at the KeyStand's position
        heldKey.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);  // Scale the key appropriately
        heldKey.tag = "Key";  // Ensure the correct tag is set for the key
        BoxCollider boxCollider = heldKey.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(-0.5f, 0f, -1.5f);
        boxCollider.size = new Vector3(8f, 2f, 8f);
        boxCollider.isTrigger = true;
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
        keyTimer = 0f;
        UpdateInstructionText("Choose a Door!");
    }

    void UpdateInstructionText(string newText)
    {
        if (instructionText != null)
        {
            instructionText.text = newText;
        }
    }

    void UpdateRoomLabel()
    {
        if (roomNumberText != null)
        {
            roomNumberText.text = "Room " + roomNumber;
        }
    }

    void ToggleInstructions()
    {
        isPaused = !isPaused;
        instructionPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
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

    void AddExtraPlant()
    {
        if (plantPrefab != null)
        {
            GameObject newPlant = Instantiate(plantPrefab, new Vector3(5.5f, 0f, 7.3f), Quaternion.identity);
            activeAnomalies.Add(newPlant); // Store reference
            Debug.Log("Anomaly Triggered: Extra Plant Added");
        }
    }




}
