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
        gameOverScreen.SetActive(false); // Asegurarse de que la pantalla de Game Over esté desactivada al inicio
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f; // Pausar el juego
    }

    void RestartLevel()
    {
        Time.timeScale = 1f; // Restaurar la escala de tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
