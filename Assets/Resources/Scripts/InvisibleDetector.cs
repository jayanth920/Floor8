using UnityEngine;

public class InvisibleDetector : MonoBehaviour
{
    
    [HideInInspector]
    public ElevatorController elevatorController;

    void Start()
    {
        //incase if it's not assigned in the Inspector
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
            elevatorController.DetectPlayerExit(gameObject);
        }
    }
}
