using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    public AudioClip hoverSound; 
    private AudioSource audioSource;
    private MusicManager musicManager;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = 1.0f;

        musicManager = FindObjectOfType<MusicManager>();

        if (hoverSound == null)
        {
            Debug.LogError("HoverSound is not assigned in the MainMenu script.");
        }

        if (musicManager == null)
        {
            Debug.LogError("MusicManager not found in the scene.");
        }
    }

    public void PlayGame()
    {
        if (musicManager != null)
        {
            musicManager.StopMusic();
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");

#if UNITY_EDITOR
     
        EditorApplication.isPlaying = false;
#else
        
        Application.Quit();
#endif
    }

    public void PlayHoverSound()
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or HoverSound is not assigned.");
        }
    }
}
