using UnityEngine;

public class RadarFollow : MonoBehaviour
{
    public Transform target; // Referencia al objeto que seguirá el radar (el jugador)
    public Vector3 offset; // Desplazamiento adicional del radar con respecto al objetivo

    void LateUpdate()
    {
        if (target != null)
        {
            // Calcula la posición deseada del radar
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

            // Establece la posición del radar
            transform.position = desiredPosition;
        }
    }
}
