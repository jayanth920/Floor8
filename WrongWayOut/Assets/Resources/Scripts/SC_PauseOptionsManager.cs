using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SC_PauseOptionsManager : MonoBehaviour
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
    if (postProcessVolume.profile.TryGet(out colorAdjustments))
    {
        // BRIGHTNESS
        float brightness = PlayerPrefs.HasKey("Brightness") ? PlayerPrefs.GetFloat("Brightness") : 0.6f;
        brightnessSlider.value = brightness;
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        SetBrightness(brightness);

        // CONTRAST
        float contrast = PlayerPrefs.HasKey("Contrast") ? PlayerPrefs.GetFloat("Contrast") : 0.5f;
        contrastSlider.value = contrast;
        contrastSlider.onValueChanged.AddListener(SetContrast);
        SetContrast(contrast);

        // COLORATION
        float coloration = PlayerPrefs.HasKey("Coloration") ? PlayerPrefs.GetFloat("Coloration") : 0.5f;
        colorationSlider.value = coloration;
        colorationSlider.onValueChanged.AddListener(SetColoration);
        SetColoration(coloration);

        // VSYNC
        bool vsync = PlayerPrefs.HasKey("VSync") ? PlayerPrefs.GetInt("VSync") == 1 : true;
        vsyncToggle.isOn = vsync;
        vsyncToggle.onValueChanged.AddListener(SetVSync);
        SetVSync(vsync);

        // VOLUME
        float volume = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : 0.5f;
        volumeSlider.value = volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
        SetVolume(volume);

        // FULLSCREEN
        bool fullscreen = PlayerPrefs.HasKey("Fullscreen") ? PlayerPrefs.GetInt("Fullscreen") == 1 : Screen.fullScreen;
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

    public void SetBrightness(float value)
    {
        // Range: -2 to 2
        colorAdjustments.postExposure.value = Mathf.Lerp(-2f, 2f, value);
        PlayerPrefs.SetFloat("Brightness", value);
        FindObjectOfType<SettingsApplierPause>()?.ApplySettings();

    }

    public void SetContrast(float value)
    {
        // Range: -100 to 100
        colorAdjustments.contrast.value = Mathf.Lerp(-100f, 100f, value);
        PlayerPrefs.SetFloat("Contrast", value);
        FindObjectOfType<SettingsApplierPause>()?.ApplySettings();

    }

    public void SetColoration(float value)
    {
        // Range: -100 (gray) to 100 (super saturated)
        colorAdjustments.saturation.value = Mathf.Lerp(-100f, 100f, value);
        PlayerPrefs.SetFloat("Coloration", value);
        FindObjectOfType<SettingsApplierPause>()?.ApplySettings();

    }

    public void SetVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isOn ? 1 : 0);
        FindObjectOfType<SettingsApplierPause>()?.ApplySettings();

    }

    public void SetVolume(float value)
    {
        // Slider from 0 to 1 → convert to dB scale
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("Volume", value);
        FindObjectOfType<SettingsApplierPause>()?.ApplySettings();

    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        FindObjectOfType<SettingsApplierPause>()?.ApplySettings();

    }

}
