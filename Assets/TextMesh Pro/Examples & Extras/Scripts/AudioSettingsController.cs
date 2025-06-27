using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSettingsController : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public TMPro.TMP_Text musicVolumeText;
    public AudioSource musicSource;

    private float savedMusicVolume;

    void Start()
    {
        savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicVolumeSlider.value = savedMusicVolume;
        UpdateMusicVolumeUI(savedMusicVolume);

        musicVolumeSlider.onValueChanged.AddListener(delegate {
            OnMusicSliderChanged();
        });
    }

    public void OnMusicSliderChanged()
    {
        float volume = musicVolumeSlider.value;
        UpdateMusicVolumeUI(volume);
        musicSource.volume = volume;
    }

    private void UpdateMusicVolumeUI(float volume)
    {
        musicVolumeText.text = Mathf.RoundToInt(volume * 100).ToString();
    }

    public void ApplySettings()
    {
        savedMusicVolume = musicVolumeSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", savedMusicVolume);
        PlayerPrefs.Save();
    }

    public void OnBack()
    {
        musicVolumeSlider.value = savedMusicVolume;
        musicSource.volume = savedMusicVolume;
        UpdateMusicVolumeUI(savedMusicVolume);
    }
}