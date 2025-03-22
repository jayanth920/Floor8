using UnityEngine;
using System.Collections;

public class Button3d : MonoBehaviour
{
    public ElevatorController elevatorController;
    public PlayerNew1 player; // Assign in Inspector
    [HideInInspector]
    public bool clicking = true; // Control clicking state
    
    // Method to handle button press action
    public void ClickButton()
    {
        if (!clicking)
        {
            return; // Do nothing if the button is disabled
        }
        
        bool isYesButton = false; // Default value
        
        // Set isYesButton based on the button name or tag
        if (gameObject.name == "YesButton") 
        {
            isYesButton = true;
        }
        else if (gameObject.name == "NoButton") 
        {
            isYesButton = false;
        }

        if (player != null)
        {
            player.VerifyAnomaly(isYesButton);  // Pass the isYesButton state to the player's method
        }

        // Trigger CloseAndOpenDoors
        if (elevatorController != null && gameObject.name != "LiftButton")
        {
            Debug.Log(gameObject.name + " pressed");
            StartCoroutine(elevatorController.CloseAndOpenDoors());  // Run coroutine to close and open doors
        }
        else if (elevatorController != null && gameObject.name == "LiftButton")
        {
            Debug.Log(gameObject.name + " pressed");
            StartCoroutine(elevatorController.OpenDoors());
        }
    }
}
