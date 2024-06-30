using UnityEngine;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit the game
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
            #endif
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // Load scene 0
            SceneManager.LoadScene(0);
        }
    }
}
