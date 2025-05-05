using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using TMPro;

public class SC_OptionsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Volume postProcessVolume;
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public Slider colorationSlider;

    public Toggle vsyncToggle;
    public Toggle fullscreenToggle;

    public Slider volumeSlider;         // Master
    public Slider musicVolumeSlider;    // Music
    public Slider sfxVolumeSlider;      // SFX

    public TMP_Dropdown resolutionDropdown;

    private readonly Resolution[] availableResolutions = new Resolution[]
    {
    new Resolution { width = 1920, height = 1080 },
    new Resolution { width = 1600, height = 900 },
    new Resolution { width = 1280, height = 720 },
    new Resolution { width = 1024, height = 576 },
    new Resolution { width = 800, height = 600 }
    };


    private ColorAdjustments colorAdjustments;

    void Start()
    {
        if (postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            // Populate resolution dropdown
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < availableResolutions.Length; i++)
            {
                Resolution res = availableResolutions[i];
                string option = res.width + " x " + res.height;
                options.Add(option);

                if (Screen.currentResolution.width == res.width && Screen.currentResolution.height == res.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
            resolutionDropdown.RefreshShownValue();
            resolutionDropdown.onValueChanged.AddListener(SetResolution);

            // Apply the selected resolution at start
            SetResolution(resolutionDropdown.value);


            // Existing settings...
            float brightness = PlayerPrefs.GetFloat("Brightness", 0.6f);
            brightnessSlider.value = brightness;
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
            SetBrightness(brightness);

            float contrast = PlayerPrefs.GetFloat("Contrast", 0.5f);
            contrastSlider.value = contrast;
            contrastSlider.onValueChanged.AddListener(SetContrast);
            SetContrast(contrast);

            float coloration = PlayerPrefs.GetFloat("Coloration", 0.5f);
            colorationSlider.value = coloration;
            colorationSlider.onValueChanged.AddListener(SetColoration);
            SetColoration(coloration);

            bool vsync = PlayerPrefs.GetInt("VSync", 1) == 1;
            vsyncToggle.isOn = vsync;
            vsyncToggle.onValueChanged.AddListener(SetVSync);
            SetVSync(vsync);

            float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
            volumeSlider.value = volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
            SetVolume(volume);

            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            musicVolumeSlider.value = musicVolume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            SetMusicVolume(musicVolume);

            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
            sfxVolumeSlider.value = sfxVolume;
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
            SetSFXVolume(sfxVolume);

            bool fullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
            fullscreenToggle.isOn = fullscreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
            SetFullscreen(fullscreen);
        }
        else
        {
            Debug.LogWarning("ColorAdjustments not found in post-process profile.");
        }
    }

    void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void SetResolution(int index)
    {
        Resolution res = availableResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        FindObjectOfType<SettingsApplier>()?.ApplySettings();
    }
    public void SetBrightness(float value)
    {
        colorAdjustments.postExposure.value = Mathf.Lerp(-2f, 2f, value);
        PlayerPrefs.SetFloat("Brightness", value);
        FindObjectOfType<SettingsApplier>()?.ApplySettings();
    }

    public void SetContrast(float value)
    {
        colorAdjustments.contrast.value = Mathf.Lerp(-100f, 100f, value);
        PlayerPrefs.SetFloat("Contrast", value);
        FindObjectOfType<SettingsApplier>()?.ApplySettings();
    }

    public void SetColoration(float value)
    {
        colorAdjustments.saturation.value = Mathf.Lerp(-100f, 100f, value);
        PlayerPrefs.SetFloat("Coloration", value);
        FindObjectOfType<SettingsApplier>()?.ApplySettings();
    }

    public void SetVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isOn ? 1 : 0);
        FindObjectOfType<SettingsApplier>()?.ApplySettings();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        FindObjectOfType<SettingsApplier>()?.ApplySettings();
    }

    public void SetVolume(float value)
    {
        float boostedValue = Mathf.Clamp(value * 2f, 0.0001f, 1f);
        float dB = Mathf.Log10(boostedValue) * 20f;
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("Volume", value);
        FindObjectOfType<SettingsApplier>()?.ApplySettings();
    }

    public void SetMusicVolume(float value)
    {
        float boostedValue = Mathf.Clamp(value * 2f, 0.0001f, 1f);
        float dB = Mathf.Log10(boostedValue) * 20f;
        audioMixer.SetFloat("MusicVolume", dB);
        PlayerPrefs.SetFloat("MusicVolume", value);
        FindObjectOfType<SettingsApplier>()?.ApplySettings();
    }

    public void SetSFXVolume(float value)
    {
        float boostedValue = Mathf.Clamp(value * 2f, 0.0001f, 1f);
        float dB = Mathf.Log10(boostedValue) * 20f;
        audioMixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat("SFXVolume", value);
        FindObjectOfType<SettingsApplier>()?.ApplySettings();
    }
}
