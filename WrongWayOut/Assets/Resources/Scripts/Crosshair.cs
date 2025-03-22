using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange = 5f;  // Maximum range for interaction
    public LayerMask interactableLayer;
    public Image crosshairImage; // The UI Image (crosshair)

    public Button3d button3d;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.green, 0.1f);

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            crosshairImage.enabled = true;  // Show crosshair when aiming at button

            if (Input.GetMouseButtonDown(0) && button3d.clicking)  // Left-click
            {
                Button3d button = hit.transform.GetComponent<Button3d>();
                if (button != null)
                {
                    button.ClickButton();  // Call PerformAction instead of OnClickAction
                }
            }
        }
        else
        {
            crosshairImage.enabled = false;  // Hide crosshair when not aiming at button
        }
    }
}
