using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResolutionDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();

        int currentResIndex = 0;
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentResIndex;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(SetResolution);
    }

    void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
