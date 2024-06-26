using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    public GameObject player;
    public GameObject laser;
    public GameObject radar;
    public GameObject inventory;
    public GameObject CheatMenu;
    public GameObject Terrain;
    public GameObject DarkCircle;
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

    private bool isPlayerDead = false;


    void Start()
    {
        laserBarController = laser.GetComponent<Laser.LaserBarController>();
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
        player.GetComponent<FuelController>().currentFuel = player.GetComponent<FuelController>().maxFuel;
    }

    public void MaxLaser()
    {
        Debug.Log("Max Laser, agregar código aquí");
    }
    public void maxHealth()
    {
        player.GetComponent<HealthController>().currentHealth = player.GetComponent<HealthController>().maxHealth;
        player.GetComponent<HealthController>().slider.value = player.GetComponent<HealthController>().currentHealth;
    }
    public void Radar()
    {
        inventory.GetComponent<InventoryManager>().AddItem("Carbon", 30, CarbonSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Piedra", 50, PiedraSprite, null);
    }

    public void SpawnRandomEnemy()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                Instantiate(MushroomPrefab, new Vector3(player.transform.position.x + 3, player.transform.position.y, player.transform.position.z), Quaternion.identity);
                break;
            case 1:
                Instantiate(BatPrefab, new Vector3(player.transform.position.x + 3, player.transform.position.y, player.transform.position.z), Quaternion.identity);
                break;
            case 2:
                Instantiate(TentaclePrefab, new Vector3(player.transform.position.x + 3, player.transform.position.y, player.transform.position.z), Quaternion.identity);
                break;
        }
    }
    public void Die()
    {
        player.GetComponent<HealthController>().Die();
        isPlayerDead = true;
    }
    public void Invincible()
    {
        player.GetComponent<HealthController>().currentHealth = 10000;
        player.GetComponent<HealthController>().slider.value = player.GetComponent<HealthController>().currentHealth;
    }
    public void TpTop()
    {
        player.transform.position = new Vector3(player.transform.position.x, Terrain.GetComponent<TerrainGeneration>().heightAddition + 20, player.transform.position.z);
    }
    public void AddAllItems()
    {
        inventory.GetComponent<InventoryManager>().AddItem("Carbon", 30, CarbonSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Piedra", 50, PiedraSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Alfa Crystals", 10, AlfaSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Cobalto", 10, CobaltoSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Ice", 10, IceSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Iron", 10, IronSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Mugufin", 10, MugufinSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Platino", 10, PlatinoSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Titanio", 10, TitanioSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Uranio", 10, UranioSprite, null);
        inventory.GetComponent<InventoryManager>().AddItem("Tungsteno", 10, TungstenoSprite, null);
    }
    public void CleanInventory()
    {
        InventoryManager inventoryManager = inventory.GetComponent<InventoryManager>();
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
    public void HideDarkCircle()
    {
        DarkCircle.GetComponent<CaveLighting>().lightCircle.SetActive(false);
        DarkCircle.GetComponent<CaveLighting>().darkCircle.SetActive(false);
        Debug.Log("Hide Dark Circle");
    }
    public void ShowDarkCircle()
    {
        DarkCircle.GetComponent<CaveLighting>().lightCircle.SetActive(true);
        DarkCircle.GetComponent<CaveLighting>().darkCircle.SetActive(true);
    }
}
