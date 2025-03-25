using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class PlayerNew1 : MonoBehaviour
{
    [HideInInspector]
    public float playerSpeed = 7.0f;
    [HideInInspector]
    public float gravity = -9.8f;
    CharacterController characterController;

    private bool hasAnomaly;
    [Header("Floor System")]
    public TextMeshPro floorText; // Assign the Floor 3D TextMeshPro
    private int currentFloor = 8;

    [Header("Anomaly Prefabs")]
    private List<GameObject> activeAnomalies = new List<GameObject>();

    [Header("Anomaly Settings")]
    public GameObject zombiePrefab;  // Assign in Inspector
    public GameObject bloodStainPrefab; // Assign in Inspector
    private string[] availableAnomalies = { "zombie", "bloodstain" };
    private string chosenAnomaly;

    private float xRotation = 0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetUpNewRoom();
    }

    void Update()
    {
        ApplyGravityWithCC();
        MouseLook();
    }

    void SetUpNewRoom()
    {
        // Clear any previous anomalies
        ClearAnomalies();

        // Randomize if there is an anomaly or not
        hasAnomaly = Random.value > 0.5f;

        Debug.Log("Anomaly: " + hasAnomaly);

        // If anomaly is true, pick a random anomaly and spawn it
        if (hasAnomaly)
        {
            chosenAnomaly = availableAnomalies[Random.Range(0, availableAnomalies.Length)];
            Debug.Log("Chosen Anomaly: " + chosenAnomaly);

            if (chosenAnomaly == "zombie")
                SpawnZombie();
            else if (chosenAnomaly == "bloodstain")
                SpawnBloodStain();
        }
        else
        {
            chosenAnomaly = ""; // No anomaly
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

    void UpdateFloorText()
    {
        if (floorText != null)
        {
            floorText.text = "Floor " + currentFloor;
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
        float mouseX = Input.GetAxis("Mouse X") * 2.0f;
        float mouseY = Input.GetAxis("Mouse Y") * 2.0f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    public void VerifyAnomaly(bool userSaidYes)
    {
        StartCoroutine(DelayedVerifyAnomaly(userSaidYes));
    }

    private IEnumerator DelayedVerifyAnomaly(bool userSaidYes)
    {
        yield return new WaitForSeconds(5f); // Wait for 2 seconds

        if ((hasAnomaly && userSaidYes) || (!hasAnomaly && !userSaidYes))
        {
            Debug.Log("Correct! Moving to the next floor.");
            currentFloor--; // Always decrease floor if correct
        }
        else
        {
            Debug.Log("Wrong! Resetting to floor 8.");
            currentFloor = 8; // Reset if wrong
        }

        UpdateFloorText();
        SetUpNewRoom();
    }

    void SpawnZombie()
    {
        if (zombiePrefab != null)
        {
            GameObject zombie = Instantiate(zombiePrefab, new Vector3(37, 0f, 7), zombiePrefab.transform.rotation);
            Debug.Log("Zombie Spawned!");
            activeAnomalies.Add(zombie);
        }
        else
        {
            Debug.LogError("Zombie Prefab or Spawn Point not assigned!");
        }
    }

    void SpawnBloodStain()
    {
        if (bloodStainPrefab != null)
        {

            Vector3 startPos = new Vector3(4.5f, 9f, 2.5f);

            GameObject bloodStain = Instantiate(bloodStainPrefab, startPos, bloodStainPrefab.transform.rotation);
            Debug.Log("Blood Stain Spawned!");

            // Add to active anomalies list
            activeAnomalies.Add(bloodStain);

        }
        else
        {
            Debug.LogError("Blood Stain Prefab not assigned!");
        }
    }



}
