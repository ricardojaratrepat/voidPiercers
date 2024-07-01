using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMusic : MonoBehaviour
{
    private static MainMenuMusic instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // Destruir el objeto duplicado
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cargar nueva escena
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Asegúrate de que este script funcione solo en el menú principal
        if (scene.name != "MainMenu")
        {
            Destroy(gameObject); // Destruir si no está en la escena del menú principal
        }
    }
}
