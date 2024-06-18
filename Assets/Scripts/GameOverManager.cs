using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
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
        gameOverScreen.SetActive(true);
        retryButton.gameObject.SetActive(true); // Show the retry button when the game is over
        Time.timeScale = 0f; // Pause the game
    }

    void RestartLevel()
    {
        Time.timeScale = 1f; // Restore the time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}