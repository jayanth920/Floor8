using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class PlayerNew1 : MonoBehaviour
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
    }

    void OnTriggerEnter(Collider other)
    {

    }



    void Respawn()
    {
        characterController.enabled = false;
        transform.position = respawnPosition;  // Respawn the player to the initial position
        characterController.enabled = true;

        SetUpNewRoom();
    }




    void SetUpNewRoom()
    {
        // Remove previous anomalies before setting up the new room
        ClearAnomalies();

        hasAnomaly = Random.value > 0.5f;
        Debug.Log("Anomaly: " + hasAnomaly);

        UpdateRoomLabel();

        // Add anomalies to the list
        anomalies.Clear();

        // If anomaly is active, pick a random one
        if (hasAnomaly && anomalies.Count > 0)
        {
            int index = Random.Range(0, anomalies.Count);
            anomalies[index].Invoke();
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






    void OnTriggerExit(Collider other)
    {

    }

    void PickupKey()
    {

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




}
