using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_PauseMainMenu : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject CreditsMenu;
    public GameObject OptionsPanel;

    public GameObject player;
    private FpsControllerWithCrosshair fpsControllerScript;

    private PlayerNew1 playerScript; 



    void Start()
    {
        PauseMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        OptionsPanel.SetActive(false);

        player = GameObject.FindWithTag("Player"); 
        if (player != null)
        {
            playerScript = player.GetComponent<PlayerNew1>();
            fpsControllerScript = player.GetComponent<FpsControllerWithCrosshair>();
        }

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

        if (fpsControllerScript != null)
        {
            fpsControllerScript.enabled = true;
        }

        
        SceneManager.UnloadSceneAsync("PauseMenu");
    }




    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }

        public void ExitToMainMenu()
    {
        if (playerScript != null)
        {
            SaveSystem.SaveFloor(playerScript.currentFloor);
            Debug.Log("Saved current floor: " + playerScript.currentFloor);
        }
        else
        {
            Debug.LogWarning("Player script not found — cannot save floor!");
        }

        Time.timeScale = 1f; // Make sure time resumes
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("StartMenu"); // Replace with your actual main menu scene name
    }
}
