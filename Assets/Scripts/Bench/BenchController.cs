using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BenchController : MonoBehaviour
{
    private TextMeshPro KeyText; // Change TextMeshProUGUI to TextMeshPro
    private bool playerInRange;
    private Canvas canvas;
    private GameObject canvasContent; // The GameObject holding all the content inside the Canvas

    // Start is called before the first frame update
    void Start()
    {
        // Busca el componente TextMeshPro dentro del prefab
        KeyText = GetComponentInChildren<TextMeshPro>(true);
        canvas = GetComponentInChildren<Canvas>(true);
        if (canvas != null)
        {
            canvasContent = canvas.transform.GetChild(0).gameObject;
            Debug.Log("Canvas component found within the prefab.");
            canvasContent.SetActive(false); // Hide canvas content initially
        }
        else
        {
            Debug.LogError("Canvas component not found within the prefab.");
        }

        if (KeyText != null)
        {
            Debug.Log("KeyText component found within the prefab.");
            KeyText.text = "";
        }
        else
        {
            Debug.LogError("KeyText component not found within the prefab.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.C))
        {
            if (canvasContent != null)
            {
                canvasContent.SetActive(!canvasContent.activeSelf); // Toggle the canvas content
                Debug.Log("Canvas content active: " + canvasContent.activeSelf);
            }
        }
        if (!playerInRange && canvasContent.activeSelf)
        {
            canvasContent.SetActive(false); // Hide canvas content if player is not in range
            Debug.Log("Canvas content active: " + canvasContent.activeSelf);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with the object");
            playerInRange = true;
            if (KeyText != null)
            {
                KeyText.text = "C";
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has exited the trigger area.");
            playerInRange = false;
            if (KeyText != null)
            {
                KeyText.text = "";
            }
        }
    }
}
