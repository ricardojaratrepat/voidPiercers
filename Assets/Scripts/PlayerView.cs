using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisibility : MonoBehaviour
{
    public float visibilityRadius = 5f; // Set the visibility radius

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, visibilityRadius);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Ore basic") || col.CompareTag("Ore medium") || col.CompareTag("Ore rare") || col.CompareTag("Ground"))
            {
                col.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        HideDistantObjects();
    }

    private void HideDistantObjects()
    {
        Collider2D[] allColliders = Physics2D.OverlapCircleAll(transform.position, visibilityRadius * 2); // Check a larger area to turn off distant blocks

        foreach (Collider2D col in allColliders)
        {
            if ((col.CompareTag("Ore basic") || col.CompareTag("Ore medium") || col.CompareTag("Ore rare") || col.CompareTag("Ground")) && Vector2.Distance(transform.position, col.transform.position) > visibilityRadius)
            {
                col.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
