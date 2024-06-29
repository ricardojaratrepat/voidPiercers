using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir este namespace para trabajar con el componente Image

public class ImageUpgradeLevel : MonoBehaviour
{
    public PlayerController playerController;
    public Image upgradeImage; // Referencia al componente Image que debe cambiar de sprite
    public Sprite sprite1; // Sprite para el nivel 1
    public Sprite sprite2; // Sprite para el nivel 2
    public Sprite sprite3; // Sprite para el nivel 3

    void Start()
    {
        if (playerController == null) {
            Debug.LogError("PlayerController is not assigned!");
            return;
        }

        if (upgradeImage == null) {
            Debug.LogError("Image component is not assigned!");
            return;
        }
    }

    void Update()
    {
        switch(playerController.ExcavationLevel)
        {
            case 1:
                upgradeImage.sprite = sprite1;
                break;
            case 2:
                upgradeImage.sprite = sprite2;
                break;
            case 3:
                upgradeImage.sprite = sprite3;
                break;
            default:
                Debug.LogError("Invalid ExcavationLevel!");
                break;
        }
    }
}
