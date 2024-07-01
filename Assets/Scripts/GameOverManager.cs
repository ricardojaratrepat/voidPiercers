using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Button retryButton;

    public AudioSource backgroundMusic; // Referencia al AudioSource de la música de fondo
    public AudioSource gameOverMusic; // Referencia al AudioSource de la música de Game Over

    private Button mainMenuButton; // Botón para volver al menú principal

    void Start()
    {
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RestartLevel);
        }

        // Encontrar el botón por su tag
        GameObject mainMenuButtonObject = GameObject.FindGameObjectWithTag("mainMenuButton");
        if (mainMenuButtonObject != null)
        {
            mainMenuButton = mainMenuButtonObject.GetComponent<Button>();
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        else
        {
            Debug.LogError("MainMenu button with tag 'mainMenuButton' not found.");
        }

        gameOverScreen.SetActive(false); // Ensure the Game Over screen is deactivated at the start
        retryButton.gameObject.SetActive(false); // Hide the retry button at the start
        if (mainMenuButton != null)
        {
            mainMenuButton.gameObject.SetActive(false); // Hide the main menu button at the start
        }
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true); // Activate the Game Over screen
        retryButton.gameObject.SetActive(true); // Show the retry button
        if (mainMenuButton != null)
        {
            mainMenuButton.gameObject.SetActive(true); // Show the main menu button
        }
        Time.timeScale = 0f; // Pause the game by setting time scale to 0

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop(); // Stop the background music
        }

        if (gameOverMusic != null)
        {
            gameOverMusic.Play(); // Play the Game Over music
        }
    }

    void RestartLevel()
    {
        Time.timeScale = 1f; // Restore normal time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene

        if (backgroundMusic != null)
        {
            backgroundMusic.Play(); // Play the background music again
        }

        if (gameOverMusic != null)
        {
            gameOverMusic.Stop(); // Stop the Game Over music
        }
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f; // Restore normal time scale
        SceneManager.LoadScene("MainMenu"); // Load the MainMenu scene

        if (backgroundMusic != null)
        {
            backgroundMusic.Play(); // Play the background music again
        }

        if (gameOverMusic != null)
        {
            gameOverMusic.Stop(); // Stop the Game Over music
        }
    }
}
