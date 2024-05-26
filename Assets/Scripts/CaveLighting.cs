using UnityEngine;

public class CaveLighting : MonoBehaviour
{
    public Transform player; // Asigna el objeto del jugador
    public GameObject lightCircle; // Asigna el sprite del círculo de luz
    public GameObject darkCircle; // Asigna el sprite del círculo oscuro
    public bool isInCave; // Determina si el jugador está en una cueva

    void Update()
    {
        if (player != null){
            lightCircle.transform.position = player.position;
            darkCircle.transform.position = player.position;
        }
        else
        {
            Debug.LogError("Referencia al jugador no establecida en CaveLighting");
        }

        if (isInCave)
        {
            // Actualiza la posición de los círculos para que sigan al jugador
            lightCircle.transform.position = player.position;
            darkCircle.transform.position = player.position;

            // Asegura que los círculos estén activos
            lightCircle.SetActive(true);
            darkCircle.SetActive(true);
        }
        else
        {
            // Desactiva los círculos cuando el jugador no está en la cueva
            lightCircle.SetActive(false);
            darkCircle.SetActive(false);
        }
    }

    // Método para actualizar el estado de isInCave desde otro script
    public void SetCaveStatus(bool status)
    {
        isInCave = status;
    }
}
