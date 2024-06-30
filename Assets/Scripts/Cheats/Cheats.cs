using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Agregar esta directiva para usar Slider

public class Cheats : MonoBehaviour
{
    public GameObject player;
    public GameObject laser;
    public GameObject radar;
    public GameObject inventory;
    public GameObject CheatMenu;
    public GameObject Terrain;
    public bool isCheating;
    public Sprite CarbonSprite;
    public Sprite PiedraSprite;
    public Sprite AlfaSprite;
    public Sprite CobaltoSprite;
    public Sprite IceSprite;
    public Sprite IronSprite;
    public Sprite MugufinSprite;
    public Sprite PlatinoSprite;
    public Sprite TitanioSprite;
    public Sprite UranioSprite;
    public Sprite TungstenoSprite;
    public Sprite emptySprite;

    private Laser.LaserBarController laserBarController;
    public GameObject MushroomPrefab;
    public GameObject BatPrefab;
    public GameObject TentaclePrefab;
    public GameObject OvenPrefab;

    private bool isPlayerDead = false;

    void Start()
    {
        if (laser != null)
        {
            laserBarController = laser.GetComponent<Laser.LaserBarController>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CheatMenu.activeSelf)
            {
                Time.timeScale = 1;
                CheatMenu.SetActive(false);
                isCheating = false;
            }
            else if (inventory.activeSelf) // Si el inventario está activo, también se cierra con Escape
            {
                inventory.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.L)) // Suponiendo que la tecla C activa/desactiva los cheats
        {
            if (isCheating)
            {
                Time.timeScale = 1;
                CheatMenu.SetActive(false);
                isCheating = false;
            }
            else
            {
                Time.timeScale = 0;
                CheatMenu.SetActive(true);
                isCheating = true;
            }

            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
            }
            else
            {
                inventory.SetActive(true);
            }
        }

        if (isPlayerDead)
        {
            Time.timeScale = 0;
        }
    }

    public void FillFuel()
    {
        if (player != null)
        {
            var fuelController = player.GetComponent<FuelController>();
            if (fuelController != null)
            {
                fuelController.currentFuel = fuelController.maxFuel;
            }
        }
    }

    public void MaxLaser()
    {
        // Resetea la duración actual del láser
        Laser.LaserState.currentDuration = 0;

        // Llama al método de actualización del slider en el LaserBarController
        if (laserBarController != null)
        {
            laserBarController.SendMessage("Update");
        }

        Debug.Log("Max Laser recargado");
    }


    public void maxHealth()
    {
        if (player != null)
        {
            var healthController = player.GetComponent<HealthController>();
            if (healthController != null)
            {
                healthController.currentHealth = healthController.maxHealth;
                healthController.slider.value = healthController.currentHealth;
            }
        }
    }

    public void Radar()
    {
        if (inventory != null)
        {
            var inventoryManager = inventory.GetComponent<InventoryManager>();
            if (inventoryManager != null)
            {
                inventoryManager.AddItem("Carbon", 30, CarbonSprite, null);
                inventoryManager.AddItem("Piedra", 50, PiedraSprite, null);
            }
        }
    }

    public void SpawnRandomEnemy()
    {
        if (player != null)
        {
            int random = Random.Range(0, 3);
            Vector3 spawnPosition = new Vector3(player.transform.position.x + 3, player.transform.position.y, player.transform.position.z);
            switch (random)
            {
                case 0:
                    Instantiate(MushroomPrefab, spawnPosition, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(BatPrefab, spawnPosition, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(TentaclePrefab, spawnPosition, Quaternion.identity);
                    break;
            }
        }
    }

    public void Die()
    {
        if (player != null)
        {
            var healthController = player.GetComponent<HealthController>();
            if (healthController != null)
            {
                healthController.Die();
                isPlayerDead = true;
            }
        }
    }

    public void Invincible()
    {
        if (player != null)
        {
            var healthController = player.GetComponent<HealthController>();
            if (healthController != null)
            {
                healthController.currentHealth = 10000;
                healthController.slider.value = healthController.currentHealth;
            }
        }
    }

    public void TpTop()
    {
        if (player != null && Terrain != null)
        {
            var terrainGen = Terrain.GetComponent<TerrainGeneration>();
            if (terrainGen != null)
            {
                player.transform.position = new Vector3(player.transform.position.x, terrainGen.heightAddition + 20, player.transform.position.z);
            }
        }
    }

    public void AddAllItems()
    {
        if (inventory != null)
        {
            var inventoryManager = inventory.GetComponent<InventoryManager>();
            if (inventoryManager != null)
            {
                inventoryManager.AddItem("Carbon", 30, CarbonSprite, null);
                inventoryManager.AddItem("Piedra", 50, PiedraSprite, null);
                inventoryManager.AddItem("Alfa Crystals", 10, AlfaSprite, null);
                inventoryManager.AddItem("Cobalto", 10, CobaltoSprite, null);
                inventoryManager.AddItem("Ice", 10, IceSprite, null);
                inventoryManager.AddItem("Iron", 10, IronSprite, null);
                inventoryManager.AddItem("Mugufin", 10, MugufinSprite, null);
                inventoryManager.AddItem("Platino", 10, PlatinoSprite, null);
                inventoryManager.AddItem("Titanio", 10, TitanioSprite, null);
                inventoryManager.AddItem("Uranio", 10, UranioSprite, null);
                inventoryManager.AddItem("Tungsteno", 10, TungstenoSprite, null);
            }
        }
    }

    public void CleanInventory()
    {
        if (inventory != null)
        {
            var inventoryManager = inventory.GetComponent<InventoryManager>();
            if (inventoryManager != null)
            {
                foreach (ItemSlot slot in inventoryManager.itemSlot)
                {
                    if (slot.quantity > 0)
                    {
                        slot.RemoveItem(slot.itemName, slot.quantity);
                    }
                }
            }
        }
    }

    public void SpawnOven()
    {
        if (player != null)
        {
            Vector3 spawnPosition = new Vector3(player.transform.position.x + 3, player.transform.position.y, player.transform.position.z);
            Instantiate(OvenPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
