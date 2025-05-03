using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private AudioSource lobbyMusicSource;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        lobbyMusicSource.volume = savedVolume;
        musicVolumeSlider.value = savedVolume;

        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    private void OnMusicVolumeChanged(float value)
    {
        lobbyMusicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}

