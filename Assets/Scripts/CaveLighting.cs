using UnityEngine;

public class CaveLighting : MonoBehaviour
{
    public Transform player; // Asigna el objeto del jugador
    public bool isInCave; // Determina si el jugador está en una cueva

    void Start()
    {
    }
    void Update()
    {
        if (player != null){
        }
        else
        {
            Debug.LogError("Referencia al jugador no establecida en CaveLighting");
        }
    }

    // Método para actualizar el estado de isInCave desde otro script
    public void SetCaveStatus(bool status)
    {
        isInCave = status;
    }
}