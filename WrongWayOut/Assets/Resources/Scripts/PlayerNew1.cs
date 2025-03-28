using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public GameObject zombiePrefab;
    public GameObject bloodStainPrefab;
    public GameObject creepyDollPrefab;
    public GameObject photoFrameHouseOnPrefab;
    public GameObject photoFrameHouseOffPrefab;

    private string[] availableAnomalies = { "zombie", "bloodstain", "creepydoll", "photoframehouseon" };
    private string chosenAnomaly;

    private List<string> anomalyHistory = new List<string>(); // Tracks last anomalies
    private int consecutiveNoAnomalies = 0; // Tracks consecutive no anomaly cases


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
        ClearAnomalies();

        // Ensure that "no anomaly" doesn't happen more than twice in a row
        if (consecutiveNoAnomalies >= 2)
        {
            hasAnomaly = true;
        }
        else
        {
            hasAnomaly = Random.value > 0.5f; // 50% chance
        }

        Debug.Log("Anomaly: " + hasAnomaly);

        if (hasAnomaly)
        {
            chosenAnomaly = GetValidAnomaly();
            Debug.Log("Chosen Anomaly: " + chosenAnomaly);

            if (chosenAnomaly == "zombie")
                SpawnZombie();
            else if (chosenAnomaly == "bloodstain")
                SpawnBloodStain();
            else if (chosenAnomaly == "creepydoll")
                SpawnCreepyDoll();
            else if (chosenAnomaly == "photoframehouseon")
                SpawnPhotoFrameHouseOn();

            anomalyHistory.Add(chosenAnomaly); // Store the anomaly
            if (anomalyHistory.Count > 4) anomalyHistory.RemoveAt(0); // Keep history size to 4
            consecutiveNoAnomalies = 0; // Reset counter
        }
        else
        {
            chosenAnomaly = "";
            consecutiveNoAnomalies++; // Increase counter
        }
    }

    void ClearAnomalies()
    {
        foreach (GameObject anomaly in activeAnomalies)
        {
            Debug.Log("Clearing Name : " + anomaly.name);
            if (chosenAnomaly == "photoframehouseon")
            {
                Debug.Log("IN THE CONDITION.");
                GameObject photoFrameHouseOff = Instantiate(photoFrameHouseOffPrefab, new Vector3(-15f, 6.5f, 0.544f), photoFrameHouseOffPrefab.transform.rotation);
                Debug.Log("photoFrameHouseOff enabled normally.");
            }

            // Destroy other anomalies
            Destroy(anomaly);
            Debug.Log(anomaly.name + " destroyed.");
        }

        // Clear the list of active anomalies
        activeAnomalies.Clear();
    }


    string GetValidAnomaly()
    {
        List<string> possibleAnomalies = new List<string>(availableAnomalies);

        // Remove any anomaly that appeared in the last 3 floors
        foreach (string recentAnomaly in anomalyHistory)
        {
            possibleAnomalies.Remove(recentAnomaly);
        }

        // If all anomalies were used recently, reset the list
        if (possibleAnomalies.Count == 0)
        {
            possibleAnomalies = new List<string>(availableAnomalies);
        }

        return possibleAnomalies[Random.Range(0, possibleAnomalies.Count)];
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

        if (currentFloor == 0)
        {
            LoadWinScene();
        }
    }

    void LoadWinScene()
    {
        Debug.Log("You won the game! Loading Win Scene...");
        SceneManager.LoadScene("Win");
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

    void SpawnCreepyDoll()
    {
        if (creepyDollPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(4.5f, 7f, 0.7f);
            GameObject creepyDoll = Instantiate(creepyDollPrefab, spawnPosition, creepyDollPrefab.transform.rotation);

            Debug.Log("Creepy Doll Spawned!");

            activeAnomalies.Add(creepyDoll);
        }
        else
        {
            Debug.LogError("Creepy Doll Prefab not assigned!");
        }
    }


    void SpawnPhotoFrameHouseOn()
    {
        GameObject photoFrameHouseOff = GameObject.Find("photoFrameHouseOff");

        if (photoFrameHouseOff != null)
        {
            Destroy(photoFrameHouseOff);
            Debug.Log("photoFrameHouseOff delete.");
        }
        else
        {
            Debug.LogWarning("photoFrameHouseOff not found.");
        }

        GameObject photoFrameHouseOn = Instantiate(photoFrameHouseOnPrefab, new Vector3(-15f, 6.5f, 0.544f), photoFrameHouseOnPrefab.transform.rotation);
        Debug.Log("photoFrameHouseOn Spawned! " + photoFrameHouseOn.name);
        activeAnomalies.Add(photoFrameHouseOn);
    }

}
