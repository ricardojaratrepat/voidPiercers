using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // ------------- Item Data -----------------
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    [SerializeField]
    private int maxNumberOfItems;

    // ------------- Item Slot -----------------
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;

    // ------------- Item Description -----------------
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;

    public GameObject selecredShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;

        this.quantity += quantity;
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        if (this.quantity >= maxNumberOfItems)
        {
            this.isFull = true;
        }

        itemImage.sprite = itemSprite;
    }

    public void RemoveItem(string itemName, int quantity)
    {
        if (itemName == null)
        {
            Debug.LogError("Item name is null.");
            return;
        }
        this.quantity -= quantity;
        quantityText.text = this.quantity.ToString();

        if (this.quantity < maxNumberOfItems)
        {
            this.isFull = false;
        }

        if (this.quantity == 0)
        {
            quantityText.enabled = false;
            this.itemName = "";
            this.itemSprite = inventoryManager.invisible;
            itemImage.sprite = inventoryManager.invisible;
            this.itemDescription = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();        
        selecredShader.SetActive(true);
        thisItemSelected = true;

        itemDescriptionNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite;

        if (itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.sprite = emptySprite;
        }
    }

    public void OnRightClick()
    {

    }
}
