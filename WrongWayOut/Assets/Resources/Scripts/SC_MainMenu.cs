using System.Collections;
using UnityEngine;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject OptionsPanel;

    private CanvasGroup mainMenuCanvasGroup;
    private CanvasGroup creditsMenuCanvasGroup;
    private CanvasGroup optionsPanelCanvasGroup;

    private void Start()
    {
        // Get the CanvasGroup components
        mainMenuCanvasGroup = MainMenu.GetComponent<CanvasGroup>();
        creditsMenuCanvasGroup = CreditsMenu.GetComponent<CanvasGroup>();
        optionsPanelCanvasGroup = OptionsPanel.GetComponent<CanvasGroup>();

        // Set initial visibility
        creditsMenuCanvasGroup.alpha = 0f;  // Make sure CreditsMenu is hidden at the start
        optionsPanelCanvasGroup.alpha = 0f; // Make sure OptionsPanel is hidden at the start

        // Ensure only MainMenu is visible at the start
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        OptionsPanel.SetActive(false);
    }

    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("WrongFloor");
    }

    public void CreditsButton()
    {
        // Fade out Main Menu and fade in Credits Menu
        StartCoroutine(FadeOutIn(mainMenuCanvasGroup, creditsMenuCanvasGroup));
    }

    public void MainMenuButton()
    {
        // Fade out current menu (either Credits or Options) and fade in Main Menu
        StartCoroutine(FadeOutIn(optionsPanelCanvasGroup, mainMenuCanvasGroup));
        StartCoroutine(FadeOutIn(creditsMenuCanvasGroup, mainMenuCanvasGroup));
    }

    public void OptionsButton()
    {
        // Fade out Main Menu and fade in Options Panel
        StartCoroutine(FadeOutIn(mainMenuCanvasGroup, optionsPanelCanvasGroup));
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    
    // Coroutine to fade out and then fade in
    private IEnumerator FadeOutIn(CanvasGroup fadeOutGroup, CanvasGroup fadeInGroup)
    {
        // Fade out the current group
        yield return StartCoroutine(FadeOut(fadeOutGroup));

        // Enable the target group
        fadeInGroup.gameObject.SetActive(true);

        // Fade in the new group
        yield return StartCoroutine(FadeIn(fadeInGroup));
    }

    // Coroutine to fade out a CanvasGroup (set alpha to 0)
    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        float duration = 0.2f; // Fade out duration in seconds

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false); // Disable after fading out
    }

    // Coroutine to fade in a CanvasGroup (set alpha to 1)
    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        float duration = 0.2f; // Fade in duration in seconds

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
