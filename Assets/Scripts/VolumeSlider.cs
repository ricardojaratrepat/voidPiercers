using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    private MusicManager musicManager;

    void Start()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError("Slider component not found on VolumeSlider object.");
        }

        musicManager = FindObjectOfType<MusicManager>();
        if (musicManager == null)
        {
            Debug.LogError("MusicManager not found in the scene.");
        }

    
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);

        slider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float volume)
    {
        if (musicManager != null)
        {
            musicManager.SetVolume(volume);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
    }
}
