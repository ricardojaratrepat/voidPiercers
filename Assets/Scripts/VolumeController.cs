using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource backgroundMusic2;
    private Slider volumeSlider;

    void Start()
    {
        // Encuentra el componente Slider en el objeto actual
        volumeSlider = GetComponent<Slider>();

        if (volumeSlider == null)
        {
            Debug.LogError("Slider component not found on VolumeController object.");
        }

        // Inicializa el volumen desde PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        SetVolume(savedVolume);

        // Configura el slider para que llame a SetVolume cuando su valor cambie
        volumeSlider.value = savedVolume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume;
        }

        if (backgroundMusic2 != null)
        {
            backgroundMusic2.volume = volume;
        }

        // Guarda el volumen en PlayerPrefs
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
