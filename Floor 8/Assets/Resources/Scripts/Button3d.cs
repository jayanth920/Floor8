using UnityEngine;

public class Button3d : MonoBehaviour
{
    public ElevatorController elevatorController;  
    public PlayerNew1 player; 
    [HideInInspector]
    public bool clicking = true;  // Control clicking state

    private void Start()
    {

        if (gameObject.activeInHierarchy)
        {
            Debug.Log(gameObject.name + " is active.");
        }
        else
        {
            Debug.LogWarning(gameObject.name + " is not active.");
        }
        // Ensure the button has a collider and it's set to interactable
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError("Button3d requires a Collider component to interact.");
        }
    }

    // Method to handle button press action
    public void ClickButton()
    {
        if (!clicking)  // Prevent action if button is not clickable
        {
            return;
        }

        bool isYesButton = false;

        // Determine button type based on its name
        if (gameObject.name == "YesButton")
        {
            isYesButton = true;
        }
        else if (gameObject.name == "NoButton")
        {
            isYesButton = false;
        }
        // Perform the button action if the player is linked
        if (player != null)
        {
            player.VerifyAnomaly(isYesButton);
        }

        // Handle door actions based on button type
        if (elevatorController != null)
        {
            if (gameObject.name != "LiftButton")
            {
                StartCoroutine(elevatorController.CloseAndOpenDoors());  // Close and open doors
            }
            else if (gameObject.name == "LiftButton" && clicking)
            {
                StartCoroutine(elevatorController.OpenDoors());  // Open doors for "LiftButton"
            }
        }
    }
}
