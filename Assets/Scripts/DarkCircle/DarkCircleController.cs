using System.Collections;
using UnityEngine;

public class DarkCircleController : MonoBehaviour
{
    private PulseEffect pulseEffect;

    void Start()
    {
        pulseEffect = GetComponent<PulseEffect>();
        if (pulseEffect == null)
        {
            Debug.LogError("No se encontró el componente PulseEffect en el darkCircle.");
        }
    }

    public void EnlargeDarkCircle()
    {
        StartCoroutine(EnlargeCoroutine());
    }

    IEnumerator EnlargeCoroutine()
    {
        if (pulseEffect != null)
        {
            // Aumenta el factor de escala al doble
            pulseEffect.scaleFactor = 2f;

            // Espera 20 segundos
            yield return new WaitForSeconds(20);

            // Restaura el factor de escala original
            pulseEffect.scaleFactor = 1f;
        }
    }
}
