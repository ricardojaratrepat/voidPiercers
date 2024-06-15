using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FuelController : MonoBehaviour
{
    public float maxFuel = 100f;
    public float currentFuel;
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        // Configura la orientación del slider a vertical
        slider.direction = Slider.Direction.BottomToTop;
        slider.maxValue = maxFuel;
        slider.value = maxFuel;
        currentFuel = maxFuel;

        // Configura el color inicial del fill
        fill.color = gradient.Evaluate(1f);
    }

    public void ConsumeFuel(float amount)
    {
        currentFuel -= amount;
        if (currentFuel <= 0)
        {
            currentFuel = 0;
            // Aquí puedes manejar lo que sucede cuando el combustible se agota
        }
        slider.value = currentFuel;

        // Evalúa el color del fill según el valor normalizado del slider
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void StartContinuousFuelConsumption(float consumptionPerSecond, float duration)
    {
        StartCoroutine(ConsumeFuelOverTime(consumptionPerSecond, duration));
    }

    private IEnumerator ConsumeFuelOverTime(float consumptionPerSecond, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            ConsumeFuel(consumptionPerSecond);
            elapsed += 1f;
            yield return new WaitForSeconds(1f);
        }
    }
}
