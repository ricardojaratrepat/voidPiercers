using UnityEngine;

public class PlayerVisibility : MonoBehaviour
{
    public float visibilityRadius = 5f; // Set the initial visibility radius
    public Color shadowColor = new Color(0, 0, 0, 0.5f); // Color for shadowed blocks

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, visibilityRadius);

        foreach (Collider2D col in colliders)
        {
            if (IsBlock(col))
            {
                col.GetComponent<SpriteRenderer>().color = Color.white; // Normal visibility
            }
        }

        ApplyShadowToDistantBlocks();
    }

    private void ApplyShadowToDistantBlocks()
    {
        Collider2D[] allColliders = Physics2D.OverlapCircleAll(transform.position, visibilityRadius * 10);

        foreach (Collider2D col in allColliders)
        {
            if (IsBlock(col) && Vector2.Distance(transform.position, col.transform.position) > visibilityRadius)
            {
                col.GetComponent<SpriteRenderer>().color = shadowColor; // Shadow effect
            }
        }
    }

    private bool IsBlock(Collider2D col)
    {
        return col.CompareTag("Ore basic") || col.CompareTag("Ore medium") || col.CompareTag("Ore rare") || col.CompareTag("Ground");
    }

    public void SetVisibilityRadius(float radius)
    {
        visibilityRadius = radius;
    }
}
