using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BenchController : MonoBehaviour
{
    private TextMeshPro KeyText; // Change TextMeshProUGUI to TextMeshPro
    private bool playerInRange;
    private Canvas canvas;
    private GameObject canvasContent; // The GameObject holding all the content inside the Canvas
    private InventoryManager inventoryManager; // Reference to InventoryManager

    // Start is called before the first frame update
    void Start()
    {
        KeyText = GetComponentInChildren<TextMeshPro>();
        canvas = GameObject.Find("BenchCanvas").GetComponent<Canvas>();
        canvasContent = canvas.transform.GetChild(0).gameObject; // Get the first child of the canvas
        canvasContent.SetActive(false); // Hide canvas content initially

        // Find the InventoryManager in the scene and get its Canvas
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.C))
        {
            ToggleBenchCanvas();
        }
    }

    private void ToggleBenchCanvas()
    {
        if (canvasContent.activeSelf)
        {
            canvasContent.SetActive(false); // Hide canvas content if it is already active
        }
        else
        {
            canvasContent.SetActive(true); // Show canvas content if player is in range and it is not active

            // Check if inventoryManager is not null before calling CloseInventory
            if (inventoryManager != null && inventoryManager.menuActivated)
            {
                inventoryManager.CloseInventory(); // Close the inventory menu
            }
            else
            {
                Debug.LogWarning("InventoryManager reference is not set in BenchController.");
            }
        }
        Debug.Log("Canvas content active: " + canvasContent.activeSelf);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with the object");
            playerInRange = true;
            KeyText.text = "C";
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has exited the trigger area.");
            playerInRange = false;
            if (KeyText != null)
            {
                KeyText.text = "";
            }
            if (canvasContent.activeSelf)
            {
                canvasContent.SetActive(false); // Hide canvas content when player exits the trigger area
            }
        }
    }
}
