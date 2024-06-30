using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    // public GameObject 
    public Button retryButton;

    void Start()
    {
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RestartLevel);
        }
        gameOverScreen.SetActive(false); // Ensure the Game Over screen is deactivated at the start
        retryButton.gameObject.SetActive(false); // Hide the retry button at the start
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true); // Activate the Game Over screen
        retryButton.gameObject.SetActive(true); // Show the retry button
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
    }

    void RestartLevel()
    {
        Time.timeScale = 1f; // Restore normal time scale
        SceneManager.LoadScene(0); // Load the main menu scene (assuming it's at build index 0)
    }
}
