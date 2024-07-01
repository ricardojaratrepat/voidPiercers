using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneMusic : MonoBehaviour
{
    private static GameSceneMusic instance;

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
        // Asegúrate de que este script funcione solo en la escena del juego
        if (scene.name != "Last") // Reemplaza "GameSceneName" con el nombre de tu escena de juego
        {
            Destroy(gameObject); // Destruir si no está en la escena del juego
        }
    }
}
