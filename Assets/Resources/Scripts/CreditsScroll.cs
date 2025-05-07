using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour
{
    public RectTransform titleText;
    public RectTransform creditsText;
    public float scrollSpeed = 20f;
    public float stopYPosition = 0f;

    public GameObject enterPrompt;
    private CanvasGroup promptCanvasGroup;

    public float fadeDuration = 1f;

    private bool hasStopped = false;
    private float fadeTimer = 0f;

    void Start()
    {
        if (enterPrompt != null)
        {
            promptCanvasGroup = enterPrompt.GetComponent<CanvasGroup>();
            if (promptCanvasGroup != null)
            {
                promptCanvasGroup.alpha = 0f;
                enterPrompt.SetActive(true); 
            }
        }
    }

    void Update()
    {
        if (!hasStopped)
        {
            titleText.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
            creditsText.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

            if (creditsText.anchoredPosition.y >= stopYPosition)
            {
                float offset = creditsText.anchoredPosition.y - stopYPosition;
                titleText.anchoredPosition -= new Vector2(0, offset);
                creditsText.anchoredPosition = new Vector2(creditsText.anchoredPosition.x, stopYPosition);

                hasStopped = true;
            }
        }
        else
        {
            if (promptCanvasGroup != null && promptCanvasGroup.alpha < 1f)
            {
                fadeTimer += Time.deltaTime;
                promptCanvasGroup.alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("StartMenu");
            }
        }
    }
}
