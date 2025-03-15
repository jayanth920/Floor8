using UnityEngine;

public class Button3d : MonoBehaviour
{
    // Reference to the ElevatorController
    public ElevatorController elevatorController;

    // Action to perform when clicked
    public void OnClickAction()
    {
        Debug.Log(gameObject.name + " clicked!");

        // Close the elevator doors when the button is clicked
        if (elevatorController != null)
        {
            elevatorController.CloseElevatorDoors();  // Trigger the door closing
        }
    }

    // Detect the mouse click
    void OnMouseDown()
    {
        // This checks if the mouse is clicked on the object with this script
        OnClickAction();
    }
}
