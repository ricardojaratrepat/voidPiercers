using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referencia al objeto que seguirá la cámara (el jugador)
    public float smoothTime = 0.3f; // Tiempo de suavizado de movimiento de la cámara
    private Vector3 velocity = Vector3.zero; // Velocidad actual de movimiento de la cámara
    public Vector3 offset; // Desplazamiento adicional de la cámara con respecto al objetivo
    public TerrainGeneration terrainGenerator; // Referencia al generador de terreno
    public int worldSize; // Tamaño del mundo

    private void Start()
    {
        // Obtiene la referencia al generador de terreno
        worldSize = terrainGenerator.worldSize;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Obtiene el tamaño del mundo del generador de terreno
            worldSize = terrainGenerator.worldSize;

            // Calcula la posición deseada de la cámara
            float desiredX = target.position.x + offset.x;
            float orthoSize = Camera.main.orthographicSize; // Asume que estás usando una cámara ortográfica
            desiredX = Mathf.Clamp(desiredX, 0 + (orthoSize * 1.8f), worldSize - (orthoSize * 1.8f));
            Vector3 desiredPosition = new Vector3(desiredX, target.position.y + offset.y, -2);
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            transform.position = smoothedPosition;

        }
    }
}
