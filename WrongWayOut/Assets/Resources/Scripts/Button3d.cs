using UnityEngine;
using System.Collections;

public class Button3d : MonoBehaviour
{
    // Reference to the ElevatorController
    public ElevatorController elevatorController;

    // Action to perform when clicked
    public void OnClickAction()
    {
        Debug.Log(gameObject.name + " button3d Working!");

        // Open the elevator doors when the button is clicked (for the LiftButton)
        if (elevatorController != null && gameObject.name != "LiftButton")
        {
            // Open doors when the button is pressed outside
            elevatorController.StartCoroutine(elevatorController.CloseAndOpenDoors());
        } else if (elevatorController != null && gameObject.name == "LiftButton")
        {
            // Open doors when the button is pressed from outside
            elevatorController.StartCoroutine(elevatorController.OpenDoors());
        }
    }

    // Detect the mouse click
    void OnMouseDown()
    {
        // This checks if the mouse is clicked on the object with this script
        OnClickAction();
    }
}
