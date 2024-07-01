using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Button retryButton;
    public AudioClip gameOverSound; // Variable para el sonido de Game Over
    private AudioSource audioSource; // Variable para el AudioSource

    void Start()
    {
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RestartLevel);
        }
        gameOverScreen.SetActive(false); // Ensure the Game Over screen is deactivated at the start
        retryButton.gameObject.SetActive(false); // Hide the retry button at the start

        audioSource = GetComponent<AudioSource>(); // Obtener el componente AudioSource
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing on the GameOverManager object.");
        }
        if (gameOverSound == null)
        {
            Debug.LogError("GameOverSound is not assigned in the GameOverManager script.");
        }
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        retryButton.gameObject.SetActive(true); // Show the retry button when the game is over
        Time.timeScale = 0f; // Pause the game
        PlayGameOverSound();
    }

    void RestartLevel()
    {
        Time.timeScale = 1f; // Restore the time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PlayGameOverSound()
    {
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
            Debug.Log("Game Over sound played.");
        }
        else
        {
            Debug.LogWarning("AudioSource or GameOverSound is not assigned.");
        }
    }
}
