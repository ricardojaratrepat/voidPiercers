using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public PlayerVisibility playerVisibility; // Reference to the PlayerVisibility component

    public float newVisibilityRadius = 10f; // Example of a new visibility radius value

    void Start()
    {
        if (playerVisibility == null)
        {
            playerVisibility = GetComponent<PlayerVisibility>(); // Try to find PlayerVisibility on the same GameObject if not set
        }

        if (playerVisibility == null)
        {
            Debug.LogError("PlayerVisibility component not found or assigned to VisibilityController.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SetNewVisibilityRadius(newVisibilityRadius); // Example: Set the visibility radius when pressing a key
        }
    }

    public void SetNewVisibilityRadius(float radius)
    {
        playerVisibility.SetVisibilityRadius(radius); // Call the method on PlayerVisibility to set the new radius
    }
}