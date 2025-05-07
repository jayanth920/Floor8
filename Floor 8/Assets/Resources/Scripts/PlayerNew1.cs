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
    public TextMeshPro floorText;
    public int currentFloor = 8;

    [Header("Anomaly Prefabs")]
    private List<GameObject> activeAnomalies = new List<GameObject>();

    [Header("Anomaly Settings")]
    public GameObject zombiePrefab;
    public GameObject bloodStainPrefab;
    public GameObject creepyDollPrefab;
    public GameObject photoFrameHouseOnPrefab;
    public GameObject photoFrameHouseOffPrefab;
    public GameObject manSitting;
    public Material normalSkinMat;
    public Material hauntedSkinMat;
    public GameObject manSitting2;
    public GameObject redLeftBallPrefab;
    public GameObject redRightBallPrefab;
    public GameObject hangmanPrefab;
    public GameObject regularEyeFrame;
    public GameObject creepyEyeFrame;
    public GameObject regularExitPrefab;
    public GameObject creepyExitPrefab;
    public GameObject regularDisturbPrefab;
    public GameObject creepyDisturbPrefab;


    public GameObject knockDetectorPrefab;
    private KnockDetector activeKnockDetector;

    public GameObject lightRoomPrefab;
    public GameObject buttonBoardPrefab;

    public GameObject ArrowPrefab;

    private string[] availableAnomalies = { "zombie", "bloodstain", "creepydoll", "photoframehouseon", "hauntedskin", "missingeyes", "hangman", "creepyeyes", "deadsign", "knocking", "disturb" };
    // private string[] availableAnomalies = { "zombie", "bloodstain", "knocking" };
    private string chosenAnomaly;

    private List<string> anomalyHistory = new List<string>(); // tracks last anomalie
    private int consecutiveNoAnomalies = 0; // tracks conscutive no anomaly cases


    private float xRotation = 0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Debug.Log("GettingFLOOR");
        currentFloor = SaveSystem.LoadFloor();
        UpdateFloorText();
        Debug.Log("Starting on floor: " + currentFloor);
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
            hasAnomaly = Random.value > 0.5f; // 50%
        }


        if (hasAnomaly)
        {
            chosenAnomaly = GetValidAnomaly();
            Debug.Log("Chosen Anomaly: " + chosenAnomaly);

            if (chosenAnomaly == "zombie")
                SpawnZombie();
            if (chosenAnomaly == "bloodstain")
                SpawnBloodStain();
            if (chosenAnomaly == "creepydoll")
                SpawnCreepyDoll();
            if (chosenAnomaly == "photoframehouseon")
                SpawnPhotoFrameHouseOn();
            if (chosenAnomaly == "hauntedskin")
                ApplyHauntedSkin();
            if (chosenAnomaly == "missingeyes")
                RemoveRedEyes();
            if (chosenAnomaly == "hangman")
                SpawnHangman();
            if (chosenAnomaly == "creepyeyes")
                SpawnCreepyEyeFrame();
            if (chosenAnomaly == "deadsign")
                SpawnCreepyExit();
            if (chosenAnomaly == "disturb")
                SpawnCreepyDisturb();
            if (chosenAnomaly == "knocking")
                SpawnKnockDetector();

            anomalyHistory.Add(chosenAnomaly); // Store the anomaly
            if (anomalyHistory.Count > 4) anomalyHistory.RemoveAt(0); // Keep history size to 4
            consecutiveNoAnomalies = 0; // Reset counter
        }
        else
        {
            chosenAnomaly = "";
            consecutiveNoAnomalies++; // Inc counter
        }
    }

    void ClearAnomalies()
    {
        Debug.Log("Chosen anomaly to clear: " + chosenAnomaly);
        if (chosenAnomaly == "photoframehouseon")
        {
            GameObject photoFrameHouseOff = Instantiate(photoFrameHouseOffPrefab, new Vector3(-15f, 6.5f, 0.544f), photoFrameHouseOffPrefab.transform.rotation);
            Debug.Log("photoFrameHouseOff enabled normally.");
        }
        if (chosenAnomaly == "hauntedskin")
        {
            SetNormalSkin();
        }
        if (chosenAnomaly == "missingeyes")
        {
            RestoreRedEyes();
        }
        if (chosenAnomaly == "creepyeyes")
        {
            GameObject regularEye = Instantiate(regularEyeFrame, regularEyeFrame.transform.position, regularEyeFrame.transform.rotation);
            Debug.Log("regularEye enabled normally.");
        }
        if (chosenAnomaly == "deadsign")
        {
            GameObject regularExit = Instantiate(regularExitPrefab, regularExitPrefab.transform.position, regularExitPrefab.transform.rotation);
            Debug.Log("regularExit enabled normally.");
        }
        if (chosenAnomaly == "disturb")
        {
            GameObject regularDisturb = Instantiate(regularDisturbPrefab, regularDisturbPrefab.transform.position, regularDisturbPrefab.transform.rotation);
            Debug.Log("regularDisturb enabled normally.");
        }
        if (chosenAnomaly == "knocking")
        {
            StopKnockDetector();
        }

        foreach (GameObject anomaly in activeAnomalies)
        {
            Destroy(anomaly);
        }

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

    public void VerifyAnomaly(bool userSaidYes)
    {
        StartCoroutine(DelayedVerifyAnomaly(userSaidYes));
    }

    private IEnumerator DelayedVerifyAnomaly(bool userSaidYes)
    {
        yield return new WaitForSeconds(5f); // Wait for 2 seconds

        if ((hasAnomaly && userSaidYes) || (!hasAnomaly && !userSaidYes))
        {
            currentFloor--; // Always decrease floor if correct
            SaveSystem.SaveFloor(currentFloor);
        }
        else
        {
            currentFloor = 8; // Reset if wrong
            SaveSystem.SaveFloor(currentFloor);
        }

        UpdateFloorText();
        SetUpNewRoom();

        if (currentFloor == 0)
        {
            ClearAnomalies();
            Debug.Log("You won the game! Loading Win Scene...");
            LoadWinScene();
        }
    }

    void LoadWinScene()
    {
        Debug.Log("in load scene");
        if (ArrowPrefab != null)
        {
            Instantiate(ArrowPrefab, ArrowPrefab.transform.position, ArrowPrefab.transform.rotation);
        }
        StartCoroutine(WaitThreeSeconds());
        DestroyStuff();
        SaveSystem.SaveFloor(8);
        if (buttonBoardPrefab != null)
        {
            Debug.Log("button board prefab deleting");
            Destroy(buttonBoardPrefab);
        }
        Instantiate(lightRoomPrefab, lightRoomPrefab.transform.position, lightRoomPrefab.transform.rotation);
    }



    IEnumerator WaitThreeSeconds()
    {
        Debug.Log("Waiting...");
        yield return new WaitForSeconds(3f);
        Debug.Log("3 seconds passed!");
    }

    public void DestroyStuff()
    {
        string[] targetNames = {
            "InvisibleBlocker", "InvisibleBlocker(Clone)",
            "InvisibleDetector", "InvisibleDetector(Clone)",
            "EleDoorLeft", "EleDoorRight"
        };

        foreach (string name in targetNames)
        {
            GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in objs)
            {
                if (obj.name == name)
                {
                    Destroy(obj);
                }
            }
        }
    }

    void SpawnZombie()
    {
        if (zombiePrefab != null)
        {
            GameObject zombie = Instantiate(zombiePrefab, new Vector3(37, 0f, 7), zombiePrefab.transform.rotation);
            activeAnomalies.Add(zombie);
        }

    }

    void SpawnBloodStain()
    {
        if (bloodStainPrefab != null)
        {

            Vector3 startPos = new Vector3(4.5f, 9f, 2.5f);

            GameObject bloodStain = Instantiate(bloodStainPrefab, startPos, bloodStainPrefab.transform.rotation);

            activeAnomalies.Add(bloodStain);

        }
    }

    void SpawnCreepyDoll()
    {
        if (creepyDollPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(4.5f, 7f, 0.7f);
            GameObject creepyDoll = Instantiate(creepyDollPrefab, spawnPosition, creepyDollPrefab.transform.rotation);

            activeAnomalies.Add(creepyDoll);
        }
    }


    void SpawnPhotoFrameHouseOn()
    {
        GameObject photoFrameHouseOff = GameObject.Find("photoFrameHouseOff");

        if (photoFrameHouseOff != null)
        {
            Destroy(photoFrameHouseOff);
        }

        GameObject photoFrameHouseOn = Instantiate(photoFrameHouseOnPrefab, new Vector3(-15f, 6.5f, 0.544f), photoFrameHouseOnPrefab.transform.rotation);
        activeAnomalies.Add(photoFrameHouseOn);
    }

    void ApplyHauntedSkin()
    {
        if (manSitting == null || hauntedSkinMat == null)
        {
            Debug.LogError("manSitting or hauntedSkinMat is not assigned!");
            return;
        }

        Transform object7 = manSitting.transform.Find("Object_7");
        if (object7 != null)
        {
            Renderer renderer = object7.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = hauntedSkinMat;
            }
        }
    }

    void SetNormalSkin()
    {
        if (manSitting == null || normalSkinMat == null)
        {
            Debug.LogError("manSitting or normalSkinMat is not assigned!");
            return;
        }

        Transform object7 = manSitting.transform.Find("Object_7");

        if (object7 != null)
        {
            Renderer renderer = object7.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = normalSkinMat;
            }
        }
    }

    void RemoveRedEyes()
    {
        Transform leftEye = manSitting2.transform.Find("LeftEye/RedLeftBall");
        Transform rightEye = manSitting2.transform.Find("RightEye/RedRightBall");

        if (leftEye != null) Destroy(leftEye.gameObject);
        if (rightEye != null) Destroy(rightEye.gameObject);
    }

    void RestoreRedEyes()
    {
        Transform leftEyeParent = manSitting2.transform.Find("LeftEye");
        Transform rightEyeParent = manSitting2.transform.Find("RightEye");

        if (leftEyeParent != null && redLeftBallPrefab != null)
        {
            GameObject newLeft = Instantiate(redLeftBallPrefab, leftEyeParent);
            newLeft.transform.localPosition = new Vector3(0f, -0.11f, 0.26f);
        }

        if (rightEyeParent != null && redRightBallPrefab != null)
        {
            GameObject newRight = Instantiate(redRightBallPrefab, rightEyeParent);
            newRight.transform.localPosition = new Vector3(0.06f, -0.11f, 0.26f);
        }
    }

    void SpawnHangman()
    {
        GameObject hangmanDrawing = Instantiate(hangmanPrefab, new Vector3(9.3f, 3.281f, 0.51f), hangmanPrefab.transform.rotation);
        activeAnomalies.Add(hangmanDrawing);
    }


    void SpawnCreepyEyeFrame()
    {
        GameObject regular = GameObject.Find("RegularEyeFrame") ?? GameObject.Find("RegularEyeFrame(Clone)");

        if (regular != null)
        {
            Destroy(regular);
        }

        GameObject creepy = Instantiate(creepyEyeFrame, creepyEyeFrame.transform.position, creepyEyeFrame.transform.rotation);
        activeAnomalies.Add(creepy);
    }

    void SpawnCreepyExit()
    {
        GameObject regularExit = GameObject.Find("Exit") ?? GameObject.Find("Exit(Clone)");

        if (regularExit != null)
        {
            Destroy(regularExit);
        }

        GameObject creepyExit = Instantiate(creepyExitPrefab, creepyExitPrefab.transform.position, creepyExitPrefab.transform.rotation);
        activeAnomalies.Add(creepyExit);
    }

    void SpawnCreepyDisturb()
    {
        GameObject regularDisturb = GameObject.Find("RegularDoorHanger") ?? GameObject.Find("RegularDoorHanger(Clone)");

        if (regularDisturb != null)
        {
            Destroy(regularDisturb);
        }

        GameObject creepyDisturb = Instantiate(creepyDisturbPrefab, creepyDisturbPrefab.transform.position, creepyDisturbPrefab.transform.rotation);
        activeAnomalies.Add(creepyDisturb);
    }

    public void SpawnKnockDetector()
    {

        if (activeKnockDetector != null)
        {
            Debug.Log("KnockDetector already spawned!");
            return;
        }


        GameObject spawnedDetector = Instantiate(knockDetectorPrefab, new Vector3(28f, 3f, 6.5f), Quaternion.Euler(0, 90, 0)); // Use player position or any spawn position
        activeKnockDetector = spawnedDetector.GetComponent<KnockDetector>();

        if (activeKnockDetector != null)
        {
            Debug.Log("Got KnockDetector");
            activeKnockDetector.isKnocking = true;
            Debug.Log("isKnocking set to true");
        }
        else
        {
            Debug.LogError("No KnockDetector script found on the spawned prefab!");
        }
    }

    public void StopKnockDetector()
    {
        if (activeKnockDetector != null)
        {
            activeKnockDetector.StopKnocking();
        }
    }

}
