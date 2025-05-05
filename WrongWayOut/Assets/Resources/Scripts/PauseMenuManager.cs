using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject player; // Reference to your player object

    private FpsControllerWithCrosshair fpsControllerScript;

    void Start()
    {
        // Get reference to the FPS controller script
        if (player != null)
        {
            fpsControllerScript = player.GetComponent<FpsControllerWithCrosshair>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseAndLoadPauseMenu();
            }
        }
    }

    private void PauseAndLoadPauseMenu()
    {
        Time.timeScale = 0f;
        isPaused = true;

        // Unlock and show the cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable the FPS controller script to stop look-around
        if (fpsControllerScript != null)
        {
            fpsControllerScript.enabled = false;
        }

        // Load the PauseMenu scene additively only if it's not already loaded
        if (!SceneManager.GetSceneByName("PauseMenu").isLoaded)
        {
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        // Lock and hide the cursor when resuming the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable the FPS controller script to allow look-around
        if (fpsControllerScript != null)
        {
            fpsControllerScript.enabled = true;
        }

        // Unload the PauseMenu scene only if it's loaded
        Scene pauseMenuScene = SceneManager.GetSceneByName("PauseMenu");
        if (pauseMenuScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(pauseMenuScene);
        }
    }
}
