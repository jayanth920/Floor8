using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange = 5f;  // Maximum range for interaction
    public LayerMask interactableLayer;
    public Image crosshairImage; // The UI Image (crosshair)

    void Update()
    {
        // Raycast directly from the camera forward
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Debug to visualize the ray in the Scene View
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.green, 0.1f);

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            crosshairImage.enabled = true;  // Show crosshair when aiming at button
            // Debug.Log("Raycast hit: " + hit.transform.name); // Debug to check what is being hit

            if (Input.GetMouseButtonDown(0))  // Left-click
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.CompareTag("YesButton"))
                {
                    Debug.Log("Yes button clicked");
                    hitObject.GetComponent<Button3d>().OnClickAction();
                }
                else if (hitObject.CompareTag("NoButton"))
                {
                    Debug.Log("No button clicked");
                    hitObject.GetComponent<Button3d>().OnClickAction();
                }
            }
        }
        else
        {
            crosshairImage.enabled = false;  // Hide crosshair when not aiming at button
        }
    }
}
