using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SpaceShipController : MonoBehaviour
{
    public PlayerController playerController;
    public InventoryManager inventory;
    public AudioClip launchSound; // Sonido del lanzamiento
    public AudioSource audioSource; // AudioSource para reproducir el sonido

    private bool playerInside;
    private TextMeshPro textInput;
    private Canvas canvas;
    private GameObject canvasContent;
    private Button button;

    private bool isLaunching = false;
    public float launchSpeed = 5f;
    public float rotationSpeed = 10f;
    public float maxRotationAngle = 15f;
    private Animator animator;
    public string mainMenuSceneName = "MainMenu";
    private Cheats cheats;

    private AudioSource backgroundMusic; // Referencia al AudioSource de la música de fondo

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        inventory = GameObject.FindObjectOfType<InventoryManager>();
        textInput = GetComponentInChildren<TextMeshPro>();
        canvas = GetComponentInChildren<Canvas>();
        canvasContent = canvas.transform.GetChild(0).gameObject;
        button = canvasContent.GetComponentInChildren<Button>();
        button.onClick.AddListener(LaunchSpaceship);
        animator = GetComponent<Animator>();
        cheats = GameObject.FindObjectOfType<Cheats>();

        canvasContent.SetActive(false);

        // Encontrar y almacenar la referencia al AudioSource de la música de fondo
        GameObject backgroundMusicObject = GameObject.Find("BackgroundMusic2");
        if (backgroundMusicObject != null)
        {
            backgroundMusic = backgroundMusicObject.GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.C))
        {
            ToggleCanvas();
        }
        if (canvasContent.activeSelf && Input.GetButtonDown("Inventory"))
        {
            canvasContent.SetActive(false);
        }

        if (isLaunching)
        {
            LaunchSequence();
        }
    }

    void ToggleCanvas()
    {
        if (InventoryManager.Instance?.MenuActivated == true)
        {
            CloseInventory();
        }
        canvasContent.SetActive(!canvasContent.activeSelf);
    }

    private void CloseInventory()
    {
        InventoryManager.Instance?.CloseInventory();
    }

    void LaunchSpaceship()
    {
        if (InventoryManager.Instance.IsAvailable("Mugufin", 10))
        {
            AlertController.Instance?.ShowGreenAlert("Launching spaceship!");
            isLaunching = true;

            animator.SetTrigger("IsLaunch");

            // Reproducir el sonido del lanzamiento
            if (audioSource != null && launchSound != null)
            {
                audioSource.PlayOneShot(launchSound);
            }

            // Detener la música de fondo
            if (backgroundMusic != null)
            {
                backgroundMusic.Stop();
            }

            // Desactivar el sprite del jugador
            if (playerController != null)
            {
                playerController.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                playerController.enabled = false; // Desactivar el control del jugador
            }

            // Cerrar el canvas
            canvasContent.SetActive(false);

            // Iniciar la secuencia de lanzamiento
            StartCoroutine(EndGame());
        }
        else
        {
            AlertController.Instance?.ShowRedAlert("You need 10 Mugufins to launch the spaceship!");
        }
    }

    void LaunchSequence()
    {
        // Mover la nave hacia arriba
        transform.Translate(Vector3.up * launchSpeed * Time.deltaTime);

        // Aplicar una pequeña rotación
        float rotationAngle = Mathf.Sin(Time.time * rotationSpeed) * maxRotationAngle;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

        // Mover al jugador con la nave
        if (playerController != null)
        {
            playerController.transform.position = transform.position;
        }
    }

    IEnumerator EndGame()
    {
        cheats.CleanInventory();
        yield return new WaitForSeconds(5f); // Esperar 5 segundos
        AlertController.Instance?.ShowGreenAlert("Congratulations! You have successfully launched the spaceship!");

        // Cargar la escena del menú principal
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInside = true;
            textInput.text = "C";
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInside = false;
            textInput.text = "";
            canvasContent.SetActive(false);
        }
    }
}
