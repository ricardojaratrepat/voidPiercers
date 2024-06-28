using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BenchController : MonoBehaviour
{
    [SerializeField] private TextMeshPro keyText;
    [SerializeField] private Canvas benchCanvas;
    [SerializeField] private GameObject canvasContent;
    [SerializeField] private Button[] upgradeButtons;

    private bool playerInRange;
    private Animator animationController;

    private const string PLAYER_TAG = "Player";
    private const string BENCH_CRAFTING_NAME = "BenchCrafting";
    private const string ITEM_PREFIX = "Item";
    private const string UPGRADE_KEY = "C";

    public bool IsBenchUsed { get; private set; }

    private enum UpgradeLevel
    {
        Level1 = 1,
        Level2 = 2,
        Level3 = 3
    }

    private void Start()
    {
        InitializeComponents();
        AssignButtonListeners();
    }

    private void Update()
    {
        HandleInput();
        CheckInventoryStatus();
    }

    private void InitializeComponents()
    {
        keyText = GetComponentInChildren<TextMeshPro>();
        benchCanvas = GetComponentInChildren<Canvas>();
        canvasContent = benchCanvas?.transform.GetChild(0).gameObject;
        animationController = GetComponent<Animator>();

        if (canvasContent == null)
        {
            throw new MissingReferenceException("Canvas content not found. Check the hierarchy.");
        }

        canvasContent.SetActive(false);

        InitializeUpgradeButtons();
    }

    private void InitializeUpgradeButtons()
    {
        var benchCrafting = FindInactiveObjectByName(canvasContent.transform, BENCH_CRAFTING_NAME);
        if (benchCrafting == null)
        {
            throw new MissingReferenceException($"'{BENCH_CRAFTING_NAME}' not found. Check the GameObject hierarchy.");
        }

        upgradeButtons = new Button[3];
        for (int i = 1; i <= 3; i++)
        {
            var item = FindInactiveObjectByName(benchCrafting, $"{ITEM_PREFIX}{i}");
            if (item == null)
            {
                throw new MissingReferenceException($"'{ITEM_PREFIX}{i}' not found under '{BENCH_CRAFTING_NAME}'. Check the GameObject hierarchy.");
            }

            upgradeButtons[i - 1] = item.GetComponentInChildren<Button>(true);
            if (upgradeButtons[i - 1] == null)
            {
                throw new MissingReferenceException($"Button component not found on '{ITEM_PREFIX}{i}'. Check the GameObject.");
            }
        }
    }

    private void AssignButtonListeners()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int level = i + 1;
            upgradeButtons[i].onClick.AddListener(() => PerformUpgrade((UpgradeLevel)level));
        }
    }

    private void HandleInput()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.C))
        {
            if (!IsBenchUsed)
            {
                ToggleCanvasContent();
            }
            else
            {
                AlertController.Instance?.ShowRedAlert("This bench has already been upgraded.");
            }
        }
    }

    private void CheckInventoryStatus()
    {
        if (InventoryManager.Instance?.MenuActivated == true && canvasContent?.activeSelf == true)
        {
            CloseCanvasContent();
        }
    }

    private void ToggleCanvasContent()
    {
        if (canvasContent.activeSelf)
        {
            CloseCanvasContent();
        }
        else
        {
            OpenCanvasContent();
        }
    }

    private void OpenCanvasContent()
    {
        if (InventoryManager.Instance?.MenuActivated == true)
        {
            CloseInventory();
        }
        benchCanvas.gameObject.SetActive(true);
        canvasContent.SetActive(true);
    }

    private void CloseCanvasContent()
    {
        canvasContent.SetActive(false);
        benchCanvas.gameObject.SetActive(false);
    }

    private void CloseInventory()
    {
        InventoryManager.Instance?.CloseInventory();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            playerInRange = true;
            keyText.text = IsBenchUsed ? "" : UPGRADE_KEY;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            playerInRange = false;
            keyText.text = "";
            CloseCanvasContent();
        }
    }

    private void PerformUpgrade(UpgradeLevel level)
    {
        if (IsBenchUsed)
        {
            AlertController.Instance?.ShowRedAlert("This bench has already been upgraded.");
            return;
        }

        var requirements = GetUpgradeRequirements(level);
        if (requirements == null) return;

        if (requirements.All(req => InventoryManager.Instance.IsAvailable(req.Key, req.Value)))
        {
            foreach (var req in requirements)
            {
                InventoryManager.Instance.RemoveItem(req.Key, req.Value);
            }
            AlertController.Instance?.ShowGreenAlert("Fuel tank filled!");
            IsBenchUsed = true;
            animationController.SetBool("IsDetroyed", true);
            CloseCanvasContent();
        }
        else
        {
            AlertController.Instance?.ShowRedAlert($"Not enough resources to upgrade. You need: {string.Join(", ", requirements.Select(req => $"{req.Key} x{req.Value}"))}");
        }
    }

    private Dictionary<string, int> GetUpgradeRequirements(UpgradeLevel level)
    {
        return level switch
        {
            UpgradeLevel.Level1 => new Dictionary<string, int> { { "Carbon", 30 }, { "Iron", 20 }, { "Piedra", 40 }, { "Alfa Crystals", 2 } },
            UpgradeLevel.Level2 => new Dictionary<string, int> { { "Carbon", 50 }, { "Iron", 30 }, { "Piedra", 60 }, { "Alfa Crystals", 3 } },
            UpgradeLevel.Level3 => new Dictionary<string, int> { { "Carbon", 70 }, { "Iron", 40 }, { "Piedra", 80 }, { "Alfa Crystals", 4 } },
            _ => null,
        };
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

}
