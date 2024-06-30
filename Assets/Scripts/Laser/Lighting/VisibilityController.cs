using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public PlayerVisibility playerVisibility; 

    public float newVisibilityRadius = 10f; 

    void Start()
    {
        if (playerVisibility == null)
        {
            playerVisibility = GetComponent<PlayerVisibility>(); 
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
            SetNewVisibilityRadius(newVisibilityRadius); 
        }
    }

    public void SetNewVisibilityRadius(float radius)
    {
        playerVisibility.SetVisibilityRadius(radius); 
    }
}