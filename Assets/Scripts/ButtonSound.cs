using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    private MainMenu mainMenu;

    void Start()
    {
        mainMenu = FindObjectOfType<MainMenu>();
        if (mainMenu == null)
        {
            Debug.LogError("MainMenu script not found in the scene.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mainMenu != null)
        {
            mainMenu.PlayHoverSound();
        }
    }
}
