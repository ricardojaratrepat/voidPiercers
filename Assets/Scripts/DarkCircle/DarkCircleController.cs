using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DarkCircleController : MonoBehaviour
{
    private PulseEffect pulseEffect;
    public GameObject lightBulbCanvas;
    private Image lightBulbImage;
    private float timer;
    private Text timerText;

    void Start()
    {
        lightBulbImage = lightBulbCanvas.GetComponentInChildren<Image>();
        timerText = lightBulbCanvas.transform.Find("Timer").GetComponent<Text>();
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
        if (pulseEffect != null && lightBulbImage != null)
        {
            // Activar el canvas
            lightBulbCanvas.SetActive(true);

            // Aumenta el factor de escala al doble
            pulseEffect.scaleFactor = 2f;

            // Inicializar el tiempo de espera
            float waitTime = 20f;

            // Mientras hay tiempo de espera
            while (waitTime > 0)
            {
                // Actualizar el texto del temporizador
                timerText.text = waitTime.ToString();

                // Esperar 1 segundo
                yield return new WaitForSeconds(1f);

                // Reducir el tiempo de espera
                waitTime--;
            }

            // Desactivar el canvas
            lightBulbCanvas.SetActive(false);

            // Restaurar el factor de escala original
            pulseEffect.scaleFactor = 1f;
        }
    }
}
