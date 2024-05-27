using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referencia al objeto que seguirá la cámara (el jugador)
    public float smoothTime = 0.3f; // Tiempo de suavizado de movimiento de la cámara
    private Vector3 velocity = Vector3.zero; // Velocidad actual de movimiento de la cámara
    public Vector3 offset; // Desplazamiento adicional de la cámara con respecto al objetivo

    void LateUpdate()
    {
        if (target != null)
        {
            // Calcula la posición deseada de la cámara
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, -2);
            
            // Aplica suavizado al movimiento de la cámara
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            
            // Establece la posición de la cámara
            transform.position = smoothedPosition;
        }
    }
}
