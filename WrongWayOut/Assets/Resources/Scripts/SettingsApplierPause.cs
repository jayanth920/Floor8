using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsApplierPause : MonoBehaviour
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

        // Volume
        float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MasterVolume", dB);

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
