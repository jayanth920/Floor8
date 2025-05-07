using UnityEngine;

public class WinMusicHandler : MonoBehaviour
{
    public PlayerNew1 player;            
    public AudioClip winMusic;        
    private AudioSource audioSource;
    private bool hasPlayed = false;

void Start()
{
    if (player == null)
    {
        Debug.LogError("Player reference not assigned in WinMusicHandler.");
        return;
    }

    audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
        audioSource = gameObject.AddComponent<AudioSource>();

    audioSource.clip = winMusic;
    audioSource.loop = true;
}


    void Update()
    {
        if (!hasPlayed && player.currentFloor == 0)
        {
            hasPlayed = true;
            audioSource.Play();
        }
    }
}
