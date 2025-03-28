using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    void Start()
    {
        // Make the cursor visible and unlock it when this scene loads
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void startGame()
    {
        Debug.Log("Starting Game");
        SceneManager.LoadScene("WrongFloor");
    }
}
