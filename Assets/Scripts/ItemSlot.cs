using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    // ------------- Item Data -----------------
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;

    // ------------- Item Slot -----------------
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;


    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        if (this.quantity > 0)
        {
            this.quantity += quantity;
            quantityText.text = this.quantity.ToString();
            return;
        }
        
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemSprite;
        itemImage.enabled = true;

        if (this.quantity >= 100)
        {
            isFull = true;
        }
    }
}
