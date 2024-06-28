using UnityEngine;

public class CaveLighting : MonoBehaviour
{
    public Transform player; // Asigna el objeto del jugador
    public GameObject lightCircle; // Asigna el sprite del círculo de luz
    public GameObject darkCircle; // Asigna el sprite del círculo oscuro
    public bool isInCave; // Determina si el jugador está en una cueva

    void Start()
    {
        darkCircle.SetActive(true);
        lightCircle.SetActive(true);
    }
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
    }

    // Método para actualizar el estado de isInCave desde otro script
    public void SetCaveStatus(bool status)
    {
        isInCave = status;
    }
}