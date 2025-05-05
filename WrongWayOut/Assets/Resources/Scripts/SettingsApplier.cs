using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsApplier : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Volume postProcessVolume;

    private ColorAdjustments colorAdjustments;


    void Awake()
    {
        Application.targetFrameRate = 60;
    }

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
        int index = PlayerPrefs.GetInt("ResolutionIndex", 0);
        Resolution[] predefinedRes = new Resolution[]
        {
    new Resolution { width = 1920, height = 1080 },
    new Resolution { width = 1600, height = 900 },
    new Resolution { width = 1280, height = 720 },
    new Resolution { width = 1024, height = 576 },
    new Resolution { width = 800, height = 600 }
};

        if (index >= 0 && index < predefinedRes.Length)
        {
            Resolution res = predefinedRes[index];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
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
