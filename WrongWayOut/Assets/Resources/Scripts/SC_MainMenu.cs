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

        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        OptionsPanel.SetActive(false);
    }

    public void PlayNowButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("WrongFloor");
    }

    public void CreditsButton()
    {
        StartCoroutine(FadeOutIn(mainMenuCanvasGroup, creditsMenuCanvasGroup));
    }

    public void MainMenuButton()
    {
        StartCoroutine(FadeOutIn(optionsPanelCanvasGroup, mainMenuCanvasGroup));
        StartCoroutine(FadeOutIn(creditsMenuCanvasGroup, mainMenuCanvasGroup));
    }

    public void OptionsButton()
    {
        StartCoroutine(FadeOutIn(mainMenuCanvasGroup, optionsPanelCanvasGroup));
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    
    private IEnumerator FadeOutIn(CanvasGroup fadeOutGroup, CanvasGroup fadeInGroup)
    {
        yield return StartCoroutine(FadeOut(fadeOutGroup));

        fadeInGroup.gameObject.SetActive(true);

        yield return StartCoroutine(FadeIn(fadeInGroup));
    }

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
        canvasGroup.gameObject.SetActive(false); 
    }

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
