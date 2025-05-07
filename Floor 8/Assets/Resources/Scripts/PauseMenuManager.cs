using UnityEngine;
using UnityEngine.EventSystems;
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
        EventSystem.current.sendNavigationEvents = false;

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

            if (Mathf.Abs(Time.timeScale - targetTimeScale) < 0.01f)
            {
                Time.timeScale = targetTimeScale;
                isTransitioning = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if PauseMenu scene is already loaded
            bool pauseMenuLoaded = SceneManager.GetSceneByName("PauseMenu").isLoaded;

            if (isPaused || pauseMenuLoaded)
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
        targetTimeScale = 1f;
        isTransitioning = true;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (fpsControllerScript != null)
        {
            fpsControllerScript.enabled = true;
        }

        Scene pauseMenuScene = SceneManager.GetSceneByName("PauseMenu");
        if (pauseMenuScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(pauseMenuScene);
        }
    }

    private void PauseAndLoadPauseMenu()
    {
        Time.timeScale = 0.1f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (fpsControllerScript != null)
        {
            fpsControllerScript.enabled = false;
        }

        if (!SceneManager.GetSceneByName("PauseMenu").isLoaded)
        {
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
        EventSystem.current.SetSelectedGameObject(null);

    }
}
