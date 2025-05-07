using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using TMPro; 

public class ScreenshotCapture : MonoBehaviour
{
    private int screenshotCount = 0;
    public TMP_Text screenshotText; 
    public CanvasGroup textCanvasGroup;
    void Start()
    {
        if (screenshotText != null)
        {
            screenshotText.text = "";
            textCanvasGroup.alpha = 0f;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveScreenshot();
        }
    }

    void SaveScreenshot()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileName = $"screenshot_{screenshotCount}.png";
        string filePath = Path.Combine(folderPath, fileName);

        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log($"📸 Screenshot saved: {filePath}");

        screenshotCount++;

        if (screenshotText != null && textCanvasGroup != null)
        {
            StopAllCoroutines(); // Stop any ongoing fades if spammed
            screenshotText.text = $"Screenshot saved at: {filePath}";
            StartCoroutine(FadeInAndOut());
        }
    }

    IEnumerator FadeInAndOut()
    {
        // Fade in
        float fadeDuration = 0.5f;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            textCanvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        textCanvasGroup.alpha = 1f;

        // Wait
        yield return new WaitForSeconds(2f);

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            textCanvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        textCanvasGroup.alpha = 0f;
    }
}
