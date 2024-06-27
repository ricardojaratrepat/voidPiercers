using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class BenchController : MonoBehaviour
{
    private TextMeshPro keyText;
    private bool playerInRange;
    private Canvas benchCanvas;
    private GameObject canvasContent; // The GameObject holding all the content inside the Canvas
    private InventoryManager inventoryManager; // Reference to InventoryManager
    private AlertController alertController;
    public bool isBenchUsed = false; // State of the bench
    private Button[] upgradeButtons;
    private Animator animationController;

    void Start()
    {
        InitializeComponents();
        canvasContent.SetActive(false);
        AssignButtonListeners();
        animationController = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.C))
        {
            if (!isBenchUsed)
            {
                if (benchCanvas != null && benchCanvas.transform.childCount > 0)
                {
                    canvasContent = benchCanvas.transform.GetChild(0).gameObject;
                    ToggleCanvasContent();
                }
            }
            else
            {
                alertController?.ShowRedAlert("This bench has already been upgraded.");
            }
        }

        // Check if the inventory is opened and close the canvasContent if it is
        if (inventoryManager != null && inventoryManager.menuActivated && canvasContent != null && canvasContent.activeSelf)
        {
            CloseCanvasContent();
        }
    }

    void ToggleCanvasContent()
    {
        if (canvasContent.activeSelf)
        {
            canvasContent.SetActive(false);
            benchCanvas.gameObject.SetActive(false); // Hide benchCanvas as well
        }
        else
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager.menuActivated)
            {
                CloseInventory();
            }
            benchCanvas.gameObject.SetActive(true);
            canvasContent.SetActive(true);
        }
    }

    void CloseInventory()
    {
        inventoryManager.InventoryMenu.SetActive(false);
        inventoryManager.menuActivated = false;
        Time.timeScale = 1f;
    }

    void CloseCanvasContent()
    {
        canvasContent.SetActive(false);
        benchCanvas.gameObject.SetActive(false);
    }

    private Transform FindInactiveObjectByName(Transform parent, string name)
    {
        if (parent.name == name)
            return parent;

        for (int i = 0; i < parent.childCount; i++)
        {
            var result = FindInactiveObjectByName(parent.GetChild(i), name);
            if (result != null)
                return result;
        }

        return null;
    }

    private void InitializeComponents()
    {
        keyText = GetComponentInChildren<TextMeshPro>();
        benchCanvas = GetComponentInChildren<Canvas>();
        canvasContent = benchCanvas.transform.GetChild(0).gameObject;

        var benchCrafting = FindInactiveObjectByName(canvasContent.transform, "BenchCrafting");
        if (benchCrafting == null) { Debug.LogError("'BenchCrafting' not found. Please check the GameObject hierarchy."); return; }

        upgradeButtons = new Button[3];
        for (int i = 1; i <= 3; i++)
        {
            var item = FindInactiveObjectByName(benchCrafting, $"Item{i}");
            if (item == null) { Debug.LogError($"'Item{i}' not found under 'BenchCrafting'. Please check the GameObject hierarchy."); return; }

            upgradeButtons[i - 1] = item.GetComponentInChildren<Button>(true);
            if (upgradeButtons[i - 1] == null) { Debug.LogError($"Button component not found on 'Item{i}'. Please check the GameObject."); return; }
        }

        alertController = FindObjectOfType<AlertController>();
        if (alertController == null) { Debug.LogError("AlertController not found. Please check the scene."); }

        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null) { Debug.LogError("InventoryManager reference is not set in BenchController."); }
    }

    private void AssignButtonListeners()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (upgradeButtons[i] != null)
            {
                int level = i + 1;
                upgradeButtons[i].onClick.AddListener(() => UpgradeLevel(level));
            }
            else
            {
                Debug.LogError($"Upgrade button {i + 1} is not assigned. Please check the inspector.");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isBenchUsed)
        {
            playerInRange = true;
            keyText.text = "C";
        }
        else if (other.gameObject.CompareTag("Player") && isBenchUsed)
        {
            playerInRange = true;
            keyText.text = "";
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            keyText.text = "";
            if (canvasContent.activeSelf)
            {
                canvasContent.SetActive(false);
                benchCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void UpgradeLevel(int level)
    {
        if (isBenchUsed)
        {
            alertController?.ShowRedAlert("This bench has already been upgraded.");
            return;
        }

        inventoryManager = FindObjectOfType<InventoryManager>();
        alertController = FindObjectOfType<AlertController>();
        if (inventoryManager == null || alertController == null) return;

        var requirements = GetUpgradeRequirements(level);
        if (requirements == null) return;

        if (requirements.All(req => inventoryManager.IsAvailable(req.Key, req.Value)))
        {
            foreach (var req in requirements)
            {
                inventoryManager.RemoveItem(req.Key, req.Value);
            }
            alertController?.ShowGreenAlert("Fuel tank filled!");
            isBenchUsed = true;
            animationController.SetBool("IsDetroyed", true);
            canvasContent.SetActive(false);
        }
        else
        {
            alertController?.ShowRedAlert($"Not enough resources to upgrade. You need: {string.Join(", ", requirements.Select(req => $"{req.Key} x{req.Value}"))}");
        }
    }

    private Dictionary<string, int> GetUpgradeRequirements(int level)
    {
        return level switch
        {
            1 => new Dictionary<string, int> { { "Carbon", 30 }, { "Iron", 20 }, { "Piedra", 40 }, { "Alfa Crystals", 2 } },
            2 => new Dictionary<string, int> { { "Carbon", 50 }, { "Iron", 30 }, { "Piedra", 60 }, { "Alfa Crystals", 3 } },
            3 => new Dictionary<string, int> { { "Carbon", 70 }, { "Iron", 40 }, { "Piedra", 80 }, { "Alfa Crystals", 4 } },
            _ => null,
        };
    }
}
