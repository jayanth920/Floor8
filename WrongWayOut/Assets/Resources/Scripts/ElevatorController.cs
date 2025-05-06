using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ElevatorController : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public float doorSpeed = 2f;

    private Vector3 leftDoorClosedPos;
    private Vector3 rightDoorClosedPos;
    private Vector3 leftDoorOpenPos;
    private Vector3 rightDoorOpenPos;

    // Blocker Prefab
    public GameObject blockerPrefab;

    // Blocker Position
    private Vector3 blockerPosition = new Vector3(4.5f, 3f, -0.35f);

    private bool isMoving = false;

    public GameObject invisiblePrefab;

    public Button3d button3d;  

    // references for open and close sounds
    public AudioSource elevatorOpenAudioSource;  // The AudioSource for opening sound
    public AudioSource elevatorCloseAudioSource; // The AudioSource for closing sound
    public AudioClip elevatorOpenSound;          // The sound to play when the elevator doors open
    public AudioClip elevatorCloseSound;         // The sound to play when the elevator doors close

    void Start()
    {
        // Store initial open positions
        leftDoorOpenPos = leftDoor.position;
        rightDoorOpenPos = rightDoor.position;

        // Define closed positions
        leftDoorClosedPos = leftDoor.position + new Vector3(3f, 0, 0);
        rightDoorClosedPos = rightDoor.position + new Vector3(-3f, 0, 0);
        StartCoroutine(CloseAndOpenDoors());
    }

    public IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            StartCoroutine(CloseAndOpenDoors());
        }
    }

    public void CloseElevatorDoors()
    {
        // Manually close the doors when this function is called
        if (!isMoving)
        {
            StartCoroutine(CloseAndOpenDoors());
        }
    }

    public IEnumerator CloseAndOpenDoors()
    {
        // Disable the button click while doors are closing/opening
        button3d.clicking = false;

        GameObject blocker = Instantiate(blockerPrefab, new Vector3(4.5f, 3f, -0.35f), Quaternion.identity);
        yield return new WaitForSeconds(1f);

        isMoving = true;

        GameObject invisibleDetector = Instantiate(invisiblePrefab, new Vector3(4f, 3f, 2f), Quaternion.identity);

        // Play the close sound when closing the doors
        if (elevatorCloseAudioSource != null && elevatorCloseSound != null)
        {
            elevatorCloseAudioSource.PlayOneShot(elevatorCloseSound);
        }

        // Close doors
        while (Vector3.Distance(leftDoor.position, leftDoorClosedPos) > 0.01f)
        {
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftDoorClosedPos, doorSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightDoorClosedPos, doorSpeed * Time.deltaTime);
            yield return null;
        }

        // Wait for 5 seconds inside the elevator
        yield return new WaitForSeconds(3f);

        // Destroy the blocker after 5 seconds
        Destroy(blocker);

        // Open doors
        if (elevatorOpenAudioSource != null && elevatorOpenSound != null)
        {
            elevatorOpenAudioSource.PlayOneShot(elevatorOpenSound);
        }

        while (Vector3.Distance(leftDoor.position, leftDoorOpenPos) > 0.01f)
        {
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftDoorOpenPos, doorSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightDoorOpenPos, doorSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;

        // Re-enable button clicking after doors are fully opened
        button3d.clicking = true;
    }

    public void DetectPlayerExit(GameObject detector)
    {
        // Close the doors when the player exits
        StartCoroutine(CloseDoors(detector));
    }

    public IEnumerator CloseDoors(GameObject invdetector)
    {
        button3d.clicking = false;

        // Destroy the passed-in invisible detector
        Destroy(invdetector);

        // Find and destroy all other invisiblePrefab instances
        GameObject[] invisibleObjects = GameObject.FindGameObjectsWithTag("Invisible");
        foreach (GameObject obj in invisibleObjects)
        {
            Destroy(obj);
        }

        GameObject blocker = Instantiate(blockerPrefab, new Vector3(4.5f, 3f, 0.3f), Quaternion.identity);
        yield return new WaitForSeconds(2f);
        isMoving = true;

        // Play the close sound when closing the doors
        if (elevatorCloseAudioSource != null && elevatorCloseSound != null)
        {
            elevatorCloseAudioSource.PlayOneShot(elevatorCloseSound);
        }

        // Close doors
        while (Vector3.Distance(leftDoor.position, leftDoorClosedPos) > 0.01f)
        {
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftDoorClosedPos, doorSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightDoorClosedPos, doorSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(blocker);

        // Just Close
        yield return true;
        isMoving = false;
        button3d.clicking = true;
    }

    public IEnumerator OpenDoors()
    {
        button3d.clicking = false;
        yield return new WaitForSeconds(1f);

        isMoving = true;
        // Play the open sound when opening the doors
        if (elevatorOpenAudioSource != null && elevatorOpenSound != null)
        {
            elevatorOpenAudioSource.PlayOneShot(elevatorOpenSound);
        }

        // Open doors
        while (Vector3.Distance(leftDoor.position, leftDoorOpenPos) > 0.01f)
        {
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftDoorOpenPos, doorSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightDoorOpenPos, doorSpeed * Time.deltaTime);
            yield return null;
        }

        // Just Open
        yield return true;
        isMoving = false;
        button3d.clicking = true;
    }
}
