using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenuManager : MonoBehaviour
{
    private bool isPaused = false;
    private bool isTransitioning = false; // To track the transition status
    private float targetTimeScale = 1f; // The target time scale when unpausing
    private float transitionSpeed = 5f; // Adjust this for smoothness

    public GameObject player;
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
        if (isTransitioning)
        {
            // Smoothly transition Time.timeScale from 0.1 to 1
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, transitionSpeed * Time.unscaledDeltaTime);

            // If the transition is close to the target, stop the transition
            if (Mathf.Abs(Time.timeScale - targetTimeScale) < 0.01f)
            {
                Time.timeScale = targetTimeScale;
                isTransitioning = false;
            }
        }

        // Escape key handling for pausing/unpausing
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

    public void ResumeGame()
    {
        // Start the smooth transition from slow-mo to normal speed
        targetTimeScale = 1f;
        isTransitioning = true;
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

    private void PauseAndLoadPauseMenu()
    {
        Time.timeScale = 0.1f; // Slow mo time dilation
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
}
