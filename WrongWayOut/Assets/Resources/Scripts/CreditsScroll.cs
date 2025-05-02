using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    public RectTransform content;
    public float scrollSpeed = 20f;
    public float waitBeforeReturn = 4f;

    void Start()
    {
        StartCoroutine(ScrollCredits());
    }

    IEnumerator ScrollCredits()
    {
        float endY = content.rect.height; // height of the credits
        while (content.anchoredPosition.y < endY)
        {
            content.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(waitBeforeReturn);
        SceneManager.LoadScene("StartMenu"); // replace with your main menu scene name
    }
}
