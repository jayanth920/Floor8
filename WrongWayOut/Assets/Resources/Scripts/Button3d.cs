using UnityEngine;
public class Button3d : MonoBehaviour
{
    // Action to perform when clicked
    public void OnClickAction()
    {
        Debug.Log(gameObject.name + " clicked!");
        // You can add more actions here (e.g., load a new scene, enable/disable objects)
    }

    // Detect the mouse click
    void OnMouseDown()
    {
        // This checks if the mouse is clicked on the object with this script
        OnClickAction();
    }
}
