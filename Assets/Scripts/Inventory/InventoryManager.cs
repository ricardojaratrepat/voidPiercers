using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public GameObject InventoryMenu;
    public bool menuActivated;
    public ItemSlot[] itemSlot;
    public Canvas Bar;
    public Canvas GameOver;
    public Sprite invisible;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Asumiendo que Bar y GameOver se asignan en el Inspector
        if (Bar == null)
        {
            Debug.LogError("Bar is not assigned in the Inspector.");
        }

        if (GameOver == null)
        {
            Debug.LogError("GameOver is not assigned in the Inspector.");
        }
    }

    public bool MenuActivated => menuActivated;

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (menuActivated)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    public void OpenInventory()
    {
        Time.timeScale = 0;

        if (InventoryMenu != null)
        {
            InventoryMenu.SetActive(true);
        }

        if (Bar != null)
        {
            Bar.enabled = false;
        }

        if (GameOver != null)
        {
            GameOver.enabled = false;
        }

        menuActivated = true;
    }

    public void CloseInventory()
    {
        Time.timeScale = 1;

        if (InventoryMenu != null)
        {
            InventoryMenu.SetActive(false);
        }

        if (Bar != null)
        {
            Bar.enabled = true;
        }

        if (GameOver != null)
        {
            GameOver.enabled = true;
        }

        menuActivated = false;
    }

    public bool IsAvailable(string itemName, int quantity)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemName == itemName && itemSlot[i].quantity >= quantity)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull && (itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0))
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                return;
            }
        }
    }

    public string RemoveItem(string itemName, int quantity)
    {
        bool IsItemAvailable = IsAvailable(itemName, quantity);

        if (!IsItemAvailable)
        {
            return "Not enough items!";
        }

        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemName == itemName)
            {
                itemSlot[i].RemoveItem(itemName, quantity);
                break;
            }
        }

        return "removed";
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selecredShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
