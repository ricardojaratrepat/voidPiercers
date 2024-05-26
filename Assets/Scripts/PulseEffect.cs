using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    public float pulseSpeed = 2.0f;  // Velocidad a la que cambia el tamaño
    public float pulseMagnitude = 0.1f;  // Cuánto cambia el tamaño
    public float stepSize = 0.05f;  // Tamaño del paso para el cambio de escala

    private Vector3 originalScale;  // Escala original para usar como base

    void Start()
    {
        originalScale = transform.localScale;  // Guarda la escala original
    }

    void Update()
    {
        float scale = Mathf.Round((1 + Mathf.Sin(Time.time * pulseSpeed) * pulseMagnitude) / stepSize) * stepSize;
        transform.localScale = originalScale * scale;  // Aplica la nueva escala basada en el tiempo y el tamaño del paso
    }
}
