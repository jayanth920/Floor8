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
    Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)); 
    RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 5f, Color.green, 0.1f);  // 5f is the range of the ray, adjust as needed


    if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
    {
        crosshairImage.enabled = true;  // Show crosshair when aiming at button

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
