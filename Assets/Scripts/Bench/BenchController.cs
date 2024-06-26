using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BenchController : MonoBehaviour
{
    private TextMeshPro keyText;
    private bool playerInRange;
    private Canvas benchCanvas;
    private GameObject canvasContent; // The GameObject holding all the content inside the Canvas
    private InventoryManager inventoryManager; // Reference to InventoryManager
    private AlertController alertController;

    void Start()
    {
        InitializeComponents();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.C))
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (canvasContent.activeSelf)
            {
                canvasContent.SetActive(false); // Hide canvas content if it's already active
            }
            else
            {
                if (inventoryManager.menuActivated)
                {
                    inventoryManager.CloseInventory();
                }
                canvasContent.SetActive(true); // Show canvas content when player is in range and presses C
            }
        }
    }

    private void InitializeComponents()
    {
        keyText = GetComponentInChildren<TextMeshPro>();
        if (keyText == null)
        {
            Debug.LogError("KeyText is not assigned. Please check the prefab.");
            return;
        }

        benchCanvas = GameObject.Find("BenchCanvas").GetComponent<Canvas>();
        if (benchCanvas == null)
        {
            Debug.LogError("BenchCanvas not found. Please check the scene.");
            return;
        }

        canvasContent = benchCanvas.transform.GetChild(0).gameObject; // Get the first child of the canvas
        canvasContent.SetActive(false); // Hide canvas content initially

        alertController = FindObjectOfType<AlertController>();
        if (alertController == null)
        {
            Debug.LogError("AlertController not found. Please check the scene.");
            return;
        }

        // Try to find the InventoryManager initially
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager reference is not set in BenchController.");
        }
        else
        {
            Debug.Log("InventoryManager reference is set in BenchController.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with the object");
            playerInRange = true;
            keyText.text = "C";
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has exited the trigger area.");
            playerInRange = false;
            keyText.text = "";
            if (canvasContent.activeSelf)
            {
                canvasContent.SetActive(false); // Hide canvas content when player exits the trigger area
            }
        }
    }

    public void UpgradeLevel1()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        alertController = FindObjectOfType<AlertController>();
        if (inventoryManager == null)
        {
            Debug.LogError("inventoryManager is not initialized.");
            return;
        }
        if (alertController == null)
        {
            Debug.LogError("alertController is not initialized.");
            return;
        }

        bool carbon = inventoryManager.IsAvailable("Carbon", 30);
        bool iron = inventoryManager.IsAvailable("Iron", 20);
        bool piedra = inventoryManager.IsAvailable("Piedra", 40);
        bool alfa = inventoryManager.IsAvailable("Alfa Crystals", 2);

        if (carbon && iron && piedra && alfa)
        {
            inventoryManager.RemoveItem("Carbon", 30);
            inventoryManager.RemoveItem("Iron", 20);
            inventoryManager.RemoveItem("Piedra", 40);
            inventoryManager.RemoveItem("Alfa Crystals", 2);
            Debug.Log("Level 1 upgraded!");
            alertController.ShowGreenAlert("Fuel tank filled!");
        }
        else
        {
            alertController.ShowRedAlert("Not enough resources to upgrade. You need: Carbon x30, Iron x20, Piedra x40, Alfa Crystals x2");
        }
    }
}
