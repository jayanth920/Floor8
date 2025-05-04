using UnityEngine;

public class SC_PauseMainMenu : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject CreditsMenu;
    public GameObject OptionsPanel;

    void Start()
    {
        PauseMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        OptionsPanel.SetActive(false);

    }

    public void CreditsButton()
    {
        // Show Credits Menu
        PauseMenu.SetActive(false);
        CreditsMenu.SetActive(true);
        OptionsPanel.SetActive(false);
    }

    public void PauseMenuButton()
    {
        // Show Pause Menu
        PauseMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        OptionsPanel.SetActive(false);
    }

    public void OptionsButton()
    {
        // Show Options Panel
        PauseMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void ResumeButton()
    {
        // Resume the game
        Time.timeScale = 1f;

        // Hide the cursor and lock it again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Unload the PauseMenu scene
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("PauseMenu");
    }




    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}
