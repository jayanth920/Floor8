using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsApplier : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Volume postProcessVolume;

    private ColorAdjustments colorAdjustments;

    void Start()
    {
        ApplySettings(); // Apply settings on scene load
    }

    public void ApplySettings()
    {
        // Fullscreen
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;

        // VSync
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSync", 0);

        // Master Volume
        float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        float masterDB = Mathf.Log10(Mathf.Clamp(volume * 2f, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MasterVolume", masterDB);

        // Music Volume
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float musicDB = Mathf.Log10(Mathf.Clamp(music * 2f, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MusicVolume", musicDB);

        // SFX Volume
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        float sfxDB = Mathf.Log10(Mathf.Clamp(sfx * 2f, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("SFXVolume", sfxDB);

        // Resolution
        int resIndex = PlayerPrefs.GetInt("ResolutionIndex", -1);
        if (resIndex >= 0 && resIndex < Screen.resolutions.Length)
        {
            Resolution res = Screen.resolutions[resIndex];
            Screen.SetResolution(res.width, res.height, isFullscreen);
        }


        // Post-processing
        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            float brightness = PlayerPrefs.GetFloat("Brightness", 0.6f);
            float contrast = PlayerPrefs.GetFloat("Contrast", 0.5f);
            float coloration = PlayerPrefs.GetFloat("Coloration", 0.5f);

            colorAdjustments.postExposure.value = Mathf.Lerp(-2f, 2f, brightness);
            colorAdjustments.contrast.value = Mathf.Lerp(-100f, 100f, contrast);
            colorAdjustments.saturation.value = Mathf.Lerp(-100f, 100f, coloration);
        }
        else
        {
            Debug.LogWarning("ColorAdjustments not found in post-processing volume.");
        }
    }
}
