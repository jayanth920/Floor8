using System.Collections;
using UnityEngine;

public class KnockDetector : MonoBehaviour
{
    public AudioSource knockingAudioSource;
    public AudioClip knockingSound;
    
    [HideInInspector]
    public bool isKnocking = false; // Set true from player when spawning

    private Coroutine knockingCoroutine;
    private bool hasTriggered = false; // So collision triggers only once

    void Start()
    {
        // In case if it's not assigned in the Inspector
        if (knockingAudioSource == null)
        {
            knockingAudioSource = GetComponent<AudioSource>();
        }else{
            Debug.Log("Check: knockingAudioSource already assigned");
        }

        Debug.Log("KnockDetectorScript started");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isKnocking && !hasTriggered)
        {
            Debug.Log("Player collided, starting knocking...");
            hasTriggered = true;
            knockingCoroutine = StartCoroutine(PlayKnockingSound());
        }
    }

    private IEnumerator PlayKnockingSound()
    {
        while (isKnocking)
        {
            knockingAudioSource.PlayOneShot(knockingSound);
            yield return new WaitForSeconds(3f);
        }
    }

    public void StopKnocking()
    {
        if (knockingCoroutine != null)
        {
            StopCoroutine(knockingCoroutine);
            knockingCoroutine = null;
        }
        isKnocking = false;

        if (knockingAudioSource.isPlaying)
        {
            knockingAudioSource.Stop();
        }

        Destroy(gameObject); // Destroy the prefab after stopping
    }
}
