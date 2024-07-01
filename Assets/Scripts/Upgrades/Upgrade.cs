using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Laser;
using System.Linq;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private PlayerController playerController;
    private LaserController laserController;
    private PlayerVisibility playerVisibility;
    public Button laser;
    public Button vision;

    void Start()
    {
        inventoryManager = InventoryManager.Instance;
        playerVisibility = FindObjectOfType<PlayerVisibility>();
        laserController = FindObjectOfType<LaserController>();
        laser.onClick.AddListener(UpgradeLaserDamage);
        vision.onClick.AddListener(UpgradeVisionRange);
    }


    public void UpgradeLaserDamage()
    {
        Dictionary<string, int> requirements = new Dictionary<string, int>
        {
            { "Alfa Crystals", 2 },
            { "Ice", 30 }
        };

        List<string> missingItems = new List<string>();

        foreach (var req in requirements)
        {
            if (!inventoryManager.IsAvailable(req.Key, req.Value))
            {
                missingItems.Add($"{req.Key} x{req.Value}");
            }
        }

        if (missingItems.Any())
        {
            AlertController.Instance?.ShowRedAlert($"Not enough resources to upgrade. You are missing: {string.Join(", ", missingItems)}");
        }
        else
        {
            // Consumir los recursos
            foreach (var req in requirements)
            {
                inventoryManager.RemoveItem(req.Key, req.Value);
            }

            // Aumentar el daño del láser
            IncreaseLaserDamage();

            AlertController.Instance?.ShowGreenAlert("Laser damage upgraded successfully!");
        }
    }

    private void IncreaseLaserDamage()
    {
        // Asumiendo que tienes una variable de daño en LaserController
        // Si no existe, necesitarás añadirla en LaserController
        if (laserController != null)
        {
            // Ejemplo: aumentar el daño en un 20%
            laserController.laserDamage *= 1.2f;
        }
        else
        {
            Debug.LogError("LaserController not found!");
        }
    }
    private void UpgradeVisionRange()
    {
        Dictionary<string, int> requirements = new Dictionary<string, int>
        {
            { "Iron", 15 },
            { "Cobalto", 15 }
        };
        List<string> missingItems = new List<string>();

        foreach (var req in requirements)
        {
            if (!inventoryManager.IsAvailable(req.Key, req.Value))
            {
                missingItems.Add($"{req.Key} x{req.Value}");
            }
        }

        if (missingItems.Any())
        {
            AlertController.Instance?.ShowRedAlert($"Not enough resources to upgrade. You are missing: {string.Join(", ", missingItems)}");
        }
        else
        {
            // Consumir los recursos
            foreach (var req in requirements)
            {
                inventoryManager.RemoveItem(req.Key, req.Value);
            }

            if (playerVisibility.visibilityRadius < 20)
            {
                playerVisibility.visibilityRadius += 1f;
                AlertController.Instance?.ShowGreenAlert("Vision upgraded successfully!");
            }
            else
            {
                AlertController.Instance?.ShowRedAlert("Cannot upgrade vision range any further!");
            }
        }
    }
}