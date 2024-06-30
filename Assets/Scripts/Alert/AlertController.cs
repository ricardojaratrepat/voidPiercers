using TMPro;
using UnityEngine;
using System.Collections;

public class AlertController : MonoBehaviour
{
    public static AlertController Instance { get; private set; }

    public TextMeshProUGUI alertText; // Referencia al TextMeshProUGUI dentro del Canvas
    public float fadeDuration = 2f; // Duración de la transición de desvanecimiento

    private Coroutine fadeCoroutine;

    // Función para mostrar un mensaje de alerta en rojo
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowRedAlert(string message)
    {
        alertText.color = Color.red;
        alertText.text = message;
        StartFadeOut();
    }

    // Función para mostrar un mensaje de alerta en verde
    public void ShowGreenAlert(string message)
    {
        alertText.color = Color.green;
        alertText.text = message;
        StartFadeOut();
    }

    // Inicia la corrutina de desvanecimiento
    private void StartFadeOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutCoroutine());
    }

    // Corrutina para el desvanecimiento suave del texto
    private IEnumerator FadeOutCoroutine()
    {
        float startAlpha = alertText.color.a;
        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            Color newColor = alertText.color;
            newColor.a = Mathf.Lerp(startAlpha, 0, t / fadeDuration);
            alertText.color = newColor;
            yield return null;
        }
        alertText.color = new Color(alertText.color.r, alertText.color.g, alertText.color.b, 0);
    }
}
