using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // Audio mixer for volume control
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropDown; // Dropdown for resolution selection
    [SerializeField] private TMPro.TMP_Dropdown qualityDropDown; // Dropdown for quality selection
    [SerializeField] private Toggle fullScreenToggle; // Toggle for fullscreen mode
    [SerializeField] private Slider volumeSlider; // Volume slider

    private Resolution[] resolutions; // Array to store available screen resolutions

    private void Start()
    {
        LoadSettings(); // Load saved settings at startup
    }

    private void LoadSettings()
    {
        //Load Volume
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0f); // Default 0 dB
        audioMixer.SetFloat("Volume", savedVolume);
        if (volumeSlider != null)
            volumeSlider.value = savedVolume;

        //Load Quality Level
        int savedQuality = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(savedQuality);
        if (qualityDropDown != null)
            qualityDropDown.value = savedQuality;

        //Load Fullscreen
        bool isFullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1; // Default to fullscreen ON
        Screen.fullScreen = isFullScreen;
        if (fullScreenToggle != null)
            fullScreenToggle.isOn = isFullScreen;

        //Load Resolution
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        int savedResolutionIndex = PlayerPrefs.GetInt("Resolution", currentResolutionIndex);
        resolutionDropDown.value = savedResolutionIndex;
        resolutionDropDown.RefreshShownValue();

        //Apply saved resolution
        SetResolution(savedResolutionIndex);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
        PlayerPrefs.SetInt("Quality", quality);
        PlayerPrefs.Save();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        PlayerPrefs.Save();
    }
}
