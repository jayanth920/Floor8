using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private bool isPaused = false;

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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Unload the PauseMenu scene only if it's loaded
        Scene pauseMenuScene = SceneManager.GetSceneByName("PauseMenu");
        if (pauseMenuScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(pauseMenuScene);
        }
    }
}
