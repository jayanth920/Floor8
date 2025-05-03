using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SC_OptionsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Volume postProcessVolume;
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public Slider colorationSlider;
    public Toggle vsyncToggle;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        // Access the ColorAdjustments from the volume
        if (postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            // Initialize Brightness
            brightnessSlider.value = 0.6f;
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
            SetBrightness(0.6f);

            // Initialize Contrast
            contrastSlider.value = 0.5f;
            contrastSlider.onValueChanged.AddListener(SetContrast);
            SetContrast(0.5f);

            // Initialize Coloration
            colorationSlider.value = 0.5f;
            colorationSlider.onValueChanged.AddListener(SetColoration);
            SetColoration(0.5f);

            // Initialize VSync
            vsyncToggle.isOn = QualitySettings.vSyncCount > 0;
            vsyncToggle.onValueChanged.AddListener(SetVSync);

            // Initialize Volume
            volumeSlider.value = 0.5f;
            volumeSlider.onValueChanged.AddListener(SetVolume);
            SetVolume(0.5f);

            // Initialize Fullscreen
            fullscreenToggle.isOn = Screen.fullScreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

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

    public void SetBrightness(float value)
    {
        // Range: -2 to 2
        colorAdjustments.postExposure.value = Mathf.Lerp(-2f, 2f, value);
        PlayerPrefs.SetFloat("Brightness", value);
    }

    public void SetContrast(float value)
    {
        // Range: -100 to 100
        colorAdjustments.contrast.value = Mathf.Lerp(-100f, 100f, value);
        PlayerPrefs.SetFloat("Contrast", value);
    }

    public void SetColoration(float value)
    {
        // Range: -100 (gray) to 100 (super saturated)
        colorAdjustments.saturation.value = Mathf.Lerp(-100f, 100f, value);
        PlayerPrefs.SetFloat("Coloration", value);
    }

    public void SetVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isOn ? 1 : 0);
    }

    public void SetVolume(float value)
    {
        // Slider from 0 to 1 → convert to dB scale
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

}
