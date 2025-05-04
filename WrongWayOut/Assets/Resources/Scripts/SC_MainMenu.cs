using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;

    public GameObject OptionsPanel;


    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("WrongFloor");
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
        OptionsPanel.SetActive(false);
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        OptionsPanel.SetActive(false);
    }

    public void OptionsButton()
    {
        // Show Options Panel
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        OptionsPanel.SetActive(true);
        
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }

    public void SetFullscreen(bool isFullscreen)
{
    Screen.fullScreen = isFullscreen;
}
}