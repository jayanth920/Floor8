using UnityEngine;
using System.Collections;

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
        GameObject blocker = Instantiate(blockerPrefab, blockerPosition, Quaternion.identity);
        yield return new WaitForSeconds(1f);

        isMoving = true;

        GameObject invisibleDetector = Instantiate(invisiblePrefab, new Vector3(4.5f, 3f, 1.5f), Quaternion.identity);


        // Close doors
        while (Vector3.Distance(leftDoor.position, leftDoorClosedPos) > 0.01f)
        {
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftDoorClosedPos, doorSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightDoorClosedPos, doorSpeed * Time.deltaTime);
            yield return null;
        }

        // Wait for 5 seconds inside the elevator
        yield return new WaitForSeconds(5f);

        // Destroy the blocker after 5 seconds
        Destroy(blocker);


        // Open doors
        while (Vector3.Distance(leftDoor.position, leftDoorOpenPos) > 0.01f)
        {
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftDoorOpenPos, doorSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightDoorOpenPos, doorSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;


    }

    public void DetectPlayerExit(GameObject i)
    {
        // Close the doors when the player exits
        StartCoroutine(CloseDoors(i));
    }

    public IEnumerator CloseDoors(GameObject i)
    {
        Destroy(i);
        GameObject blocker = Instantiate(blockerPrefab, new Vector3(4.5f, 3f, 0.5f), Quaternion.identity);
        yield return new WaitForSeconds(2f);
        isMoving = true;

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
    }

    public IEnumerator OpenDoors()
    {
        yield return new WaitForSeconds(1f);

        isMoving = true;
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
    }
}