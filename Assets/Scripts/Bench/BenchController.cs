using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BenchController : MonoBehaviour
{
    private TextMeshPro KeyText; // Change TextMeshProUGUI to TextMeshPro

    // Start is called before the first frame update
    void Start()
    {
        // Busca el componente TextMeshPro dentro del prefab
        KeyText = GetComponentInChildren<TextMeshPro>(true);
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

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with the object");
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
            if (KeyText != null)
            {
                KeyText.text = "";
            }
        }
    }
}
