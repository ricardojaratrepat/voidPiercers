using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivared;

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
}
