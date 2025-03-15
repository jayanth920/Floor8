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

    IEnumerator CloseAndOpenDoors()
    {
        isMoving = true;

        // Close doors
        while (Vector3.Distance(leftDoor.position, leftDoorClosedPos) > 0.01f)
        {
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftDoorClosedPos, doorSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightDoorClosedPos, doorSpeed * Time.deltaTime);
            yield return null;
        }

        // Wait for 5 seconds inside the elevator
        yield return new WaitForSeconds(5f);

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
