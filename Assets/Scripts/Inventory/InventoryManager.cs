using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivared;
    public ItemSlot[] itemSlot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Inventory") && menuActivared)
        {
            // Resumes the game
            Time.timeScale = 1;

            InventoryMenu.SetActive(false);
            menuActivared = false;
        }
        else if(Input.GetButtonDown("Inventory") && !menuActivared)
        {
            // Stops the game. (This can create problems if there is any animaiton in the manu)
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivared = true;
        }
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
            if (itemSlot[i].isFull == false && itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0)
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
