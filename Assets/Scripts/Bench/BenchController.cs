using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BenchController : MonoBehaviour
{
    private TextMeshPro keyText;
    private bool playerInRange;
    private Canvas benchCanvas;
    private GameObject canvasContent; // The GameObject holding all the content inside the Canvas
    private InventoryManager inventoryManager; // Reference to InventoryManager
    private AlertController alertController;
    public bool isBenchUsed = false; // State of the bench
    private Button upgradeButton1;
    private Button upgradeButton2;
    private Button upgradeButton3;

    void Start()
    {
        InitializeComponents();
        canvasContent.SetActive(false);

        if (upgradeButton1 != null)
        {
            Debug.Log("Upgrade button found and assigned.");
            upgradeButton1.onClick.AddListener(UpgradeLevel1);
        }
        else
        {
            Debug.LogError("Upgrade button is not assigned. Please check the inspector.");
        }
        if (upgradeButton2 != null)
        {
            Debug.Log("Upgrade button found and assigned.");
            upgradeButton2.onClick.AddListener(UpgradeLevel2);
        }
        else
        {
            Debug.LogError("Upgrade button is not assigned. Please check the inspector.");
        }
        if (upgradeButton3 != null)
        {
            Debug.Log("Upgrade button found and assigned.");
            upgradeButton3.onClick.AddListener(UpgradeLevel3);
        }
        else
        {
            Debug.LogError("Upgrade button is not assigned. Please check the inspector.");
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.C))
        {
            if (canvasContent.activeSelf)
            {
                canvasContent.SetActive(false); // Hide canvas content if it's already active
            }
            else
            {
                inventoryManager = FindObjectOfType<InventoryManager>();
                if (inventoryManager.menuActivated)
                {
                    CloseInventory();
                }
                benchCanvas.gameObject.SetActive(true); // Show benchCanvas when player is in range and presses C
                canvasContent.SetActive(true); // Show canvas content
            }
        }
    }

    void CloseInventory()
    {
        inventoryManager.InventoryMenu.SetActive(false);
        inventoryManager.menuActivated = false;
    }

    private Transform FindInactiveObjectByName(Transform parent, string name)
    {
        // Check the current parent
        if (parent.name == name)
            return parent;

        // Recursively check all children
        for (int i = 0; i < parent.childCount; i++)
        {
            var result = FindInactiveObjectByName(parent.GetChild(i), name);
            if (result != null)
                return result;
        }

        // Return null if the object with the specified name was not found
        return null;
    }

    private void InitializeComponents()
    {
        keyText = GetComponentInChildren<TextMeshPro>();
        if (keyText == null)
        {
            Debug.LogError("KeyText is not assigned. Please check the prefab.");
            return;
        }

        benchCanvas = GetComponentInChildren<Canvas>();
        if (benchCanvas == null)
        {
            Debug.LogError("BenchCanvas not found. Please check the scene.");
            return;
        }

        canvasContent = benchCanvas.transform.GetChild(0).gameObject; // Get the first child of the canvas

        // Use the method to find 'BenchCrafting' even if inactive
        var benchCrafting = FindInactiveObjectByName(canvasContent.transform, "BenchCrafting");
        if (benchCrafting == null)
        {
            Debug.LogError("'BenchCrafting' not found. Please check the GameObject hierarchy.");
            return;
        }
        else
        {
            Debug.Log("'BenchCrafting' found successfully.");
        }

        // Use the method to find 'Item1' even if inactive
        var item1 = FindInactiveObjectByName(benchCrafting, "Item1");
        if (item1 == null)
        {
            Debug.LogError("'item1' not found under 'BenchCrafting'. Please check the GameObject hierarchy.");
            return;
        }
        else
        {
            Debug.Log("'item1' found under 'BenchCrafting'.");
        }
        var item2 = FindInactiveObjectByName(benchCrafting, "Item2");
        if (item2 == null)
        {
            Debug.LogError("'item2' not found under 'BenchCrafting'. Please check the GameObject hierarchy.");
            return;
        }
        else
        {
            Debug.Log("'item2' found under 'BenchCrafting'.");
        }
        var item3 = FindInactiveObjectByName(benchCrafting, "Item3");
        if (item3 == null)
        {
            Debug.LogError("'item3' not found under 'BenchCrafting'. Please check the GameObject hierarchy.");
            return;
        }
        else
        {
            Debug.Log("'item3' found under 'BenchCrafting'.");
        }

        // Attempt to get the Button component from 'item1'
        var button1 = item1.GetComponentInChildren<Button>(true); // Use 'true' to include inactive children
        if (button1 != null)
        {
            // Button found and assigned successfully
            upgradeButton1 = button1;
            Debug.Log("Button component found on 'item1' and assigned.");
        }
        else
        {
            Debug.LogError("Button component not found on 'item1'. Please check the GameObject.");
        }
        var button2 = item2.GetComponentInChildren<Button>(true); // Use 'true' to include inactive children
        if (button2 != null)
        {
            // Button found and assigned successfully
            upgradeButton2 = button2;
            Debug.Log("Button component found on 'item2' and assigned.");
        }
        else
        {
            Debug.LogError("Button component not found on 'item2'. Please check the GameObject.");
        }
        var button3 = item3.GetComponentInChildren<Button>(true); // Use 'true' to include inactive children
        if (button3 != null)
        {
            // Button found and assigned successfully
            upgradeButton3 = button3;
            Debug.Log("Button component found on 'item3' and assigned.");
        }
        else
        {
            Debug.LogError("Button component not found on 'item3'. Please check the GameObject.");
        }

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
                benchCanvas.gameObject.SetActive(false); // Also hide the benchCanvas
            }
        }
    }

    public void UpgradeLevel1()
    {
        Debug.Log("Upgrade button clicked.");
        
        if (isBenchUsed)
        {
            if (alertController != null)
            {
                alertController.ShowRedAlert("This bench has already been upgraded.");
            }
            Debug.Log($"Bench {gameObject.name} upgrade attempt failed. isBenchUsed = {isBenchUsed}");
            return;
        }

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
            if (alertController != null)
            {
                alertController.ShowGreenAlert("Fuel tank filled!");
            }
            isBenchUsed = true; // Set the state to indicate the bench has been used
            Debug.Log($"Bench {gameObject.name} upgraded successfully. isBenchUsed = {isBenchUsed}");
        }
        else
        {
            if (alertController != null)
            {
                alertController.ShowRedAlert("Not enough resources to upgrade. You need: Carbon x30, Iron x20, Piedra x40, Alfa Crystals x2");
            }
            Debug.Log($"Bench {gameObject.name} upgrade attempt failed due to insufficient resources. isBenchUsed = {isBenchUsed}");
        }
    }
    public void UpgradeLevel2(){
        Debug.Log("Upgrade button clicked.");
        
        if (isBenchUsed)
        {
            if (alertController != null)
            {
                alertController.ShowRedAlert("This bench has already been upgraded.");
            }
            Debug.Log($"Bench {gameObject.name} upgrade attempt failed. isBenchUsed = {isBenchUsed}");
            return;
        }

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

        bool carbon = inventoryManager.IsAvailable("Carbon", 50);
        bool iron = inventoryManager.IsAvailable("Iron", 30);
        bool piedra = inventoryManager.IsAvailable("Piedra", 60);
        bool alfa = inventoryManager.IsAvailable("Alfa Crystals", 3);

        if (carbon && iron && piedra && alfa)
        {
            inventoryManager.RemoveItem("Carbon", 50);
            inventoryManager.RemoveItem("Iron", 30);
            inventoryManager.RemoveItem("Piedra", 60);
            inventoryManager.RemoveItem("Alfa Crystals", 3);
            Debug.Log("Level 2 upgraded!");
            if (alertController != null)
            {
                alertController.ShowGreenAlert("Fuel tank filled!");
            }
            isBenchUsed = true; // Set the state to indicate the bench has been used
            Debug.Log($"Bench {gameObject.name} upgraded successfully. isBenchUsed = {isBenchUsed}");
        }
        else
        {
            if (alertController != null)
            {
                alertController.ShowRedAlert("Not enough resources to upgrade. You need: Carbon x50, Iron x30, Piedra x60, Alfa Crystals x3");
            }
            Debug.Log($"Bench {gameObject.name} upgrade attempt failed due to insufficient resources. isBenchUsed = {isBenchUsed}");
        }
    }
    public void UpgradeLevel3()
    {
        Debug.Log("Upgrade button clicked.");
        
        if (isBenchUsed)
        {
            if (alertController != null)
            {
                alertController.ShowRedAlert("This bench has already been upgraded.");
            }
            Debug.Log($"Bench {gameObject.name} upgrade attempt failed. isBenchUsed = {isBenchUsed}");
            return;
        }

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

        bool carbon = inventoryManager.IsAvailable("Carbon", 70);
        bool iron = inventoryManager.IsAvailable("Iron", 40);
        bool piedra = inventoryManager.IsAvailable("Piedra", 80);
        bool alfa = inventoryManager.IsAvailable("Alfa Crystals", 4);

        if (carbon && iron && piedra && alfa)
        {
            inventoryManager.RemoveItem("Carbon", 70);
            inventoryManager.RemoveItem("Iron", 40);
            inventoryManager.RemoveItem("Piedra", 80);
            inventoryManager.RemoveItem("Alfa Crystals", 4);
            Debug.Log("Level 3 upgraded!");
            if (alertController != null)
            {
                alertController.ShowGreenAlert("Fuel tank filled!");
            }
            isBenchUsed = true; // Set the state to indicate the bench has been used
            Debug.Log($"Bench {gameObject.name} upgraded successfully. isBenchUsed = {isBenchUsed}");
        }
        else
        {
            if (alertController != null)
            {
                alertController.ShowRedAlert("Not enough resources to upgrade. You need: Carbon x70, Iron x40, Piedra x80, Alfa Crystals x4");
            }
            Debug.Log($"Bench {gameObject.name} upgrade attempt failed due to insufficient resources. isBenchUsed = {isBenchUsed}");
        }
    }
}
