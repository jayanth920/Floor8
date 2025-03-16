using UnityEngine;

public class InvisibleDetector : MonoBehaviour
{
    // Reference to the ElevatorController
    public ElevatorController elevatorController;

    void Start()
    {
        // Assign ElevatorController if it's not assigned in the Inspector
        if (elevatorController == null)
        {
            elevatorController = FindFirstObjectByType<ElevatorController>();
        }
    }

    // When the player exits (touches this detector), close the doors
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Call the method in ElevatorController to close the doors
            elevatorController.DetectPlayerExit(gameObject);
        }
    }
}
