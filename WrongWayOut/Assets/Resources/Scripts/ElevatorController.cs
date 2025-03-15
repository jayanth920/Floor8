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
    private Vector3 blockerPosition = new Vector3(4.5f, 3f, -0.2f);

    private bool isMoving = false;

    void Start()
    {
        // Store initial open positions
        leftDoorOpenPos = leftDoor.position;
        rightDoorOpenPos = rightDoor.position;

        // Define closed positions
        leftDoorClosedPos = leftDoor.position + new Vector3(3f, 0, 0);
        rightDoorClosedPos = rightDoor.position + new Vector3(-3f, 0, 0);
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

    IEnumerator CloseAndOpenDoors()
    {
        isMoving = true;

        GameObject blocker = Instantiate(blockerPrefab, blockerPosition, Quaternion.identity);


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
}
