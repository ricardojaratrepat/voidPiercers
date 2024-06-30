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
    public Button laser;

    void Start()
    {
        inventoryManager = InventoryManager.Instance;
        playerController = FindObjectOfType<PlayerController>();
        laserController = FindObjectOfType<LaserController>();
        laser.onClick.AddListener(UpgradeLaserDamage);
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
}