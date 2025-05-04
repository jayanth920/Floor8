using UnityEngine;
using UnityEngine.Audio;

public class VolumeLoader : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Start()
    {
        ApplyVolume("MasterVolume", "Volume", "MuteMaster");
        ApplyVolume("MusicVolume", "MusicVolume", "MuteMusic");
        ApplyVolume("SFXVolume", "SFXVolume", "MuteSFX");
    }

    void ApplyVolume(string mixerParameter, string playerPrefKey, string mutePrefKey)
    {
        bool isMuted = PlayerPrefs.GetInt(mutePrefKey, 0) == 1;
        float savedVolume = PlayerPrefs.GetFloat(playerPrefKey, 0.5f);

        if (isMuted)
        {
            audioMixer.SetFloat(mixerParameter, -80f); // Effectively mute
        }
        else
        {
            // Boost the volume even more
            float adjustedVolume = savedVolume * 50f; // Much higher boost factor
            adjustedVolume = Mathf.Clamp(adjustedVolume, 0.0001f, 1f); // Clamp to avoid log(0)

            // Convert to dB scale
            float dB = Mathf.Log10(adjustedVolume) * 20f;
            audioMixer.SetFloat(mixerParameter, dB);

            // Debugging: log the values to check if they are being set correctly
            Debug.Log($"Setting {mixerParameter} to {dB} dB (Volume: {savedVolume}, Adjusted: {adjustedVolume})");
        }
    }
}
