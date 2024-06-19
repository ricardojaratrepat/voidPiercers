using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelController : MonoBehaviour
{
    public float maxFuel = 100f;
    public float currentFuel;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public InventoryManager inventoryManager;
    public Button FillFuelButton;
    public AlertController alertController;
    public GameOverManager gameOverManager;

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        // Configura la orientación del slider a vertical
        slider.maxValue = maxFuel;
        slider.value = maxFuel;
        currentFuel = maxFuel;

        // Configura el color inicial del fill
        fill.color = gradient.Evaluate(1f);

        if (FillFuelButton != null)
        {
            FillFuelButton.onClick.AddListener(FillFuelTank);
        }
    }

    public void ConsumeFuel(float amount)
    {
        currentFuel -= amount;
        if (currentFuel <= 0)
        {
            currentFuel = 0;
            gameOverManager.GameOver();
            alertController.ShowRedAlert("Fuel tank empty!");
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

    void FillFuelTank()
    {
        bool carbon = inventoryManager.IsAvailable("Carbon", 30);
        bool cobalto = inventoryManager.IsAvailable("Cobalto", 15);

        if (carbon && cobalto)
        {
            currentFuel = maxFuel;
            inventoryManager.RemoveItem("Carbon", 30);
            inventoryManager.RemoveItem("Cobalto", 15);
            slider.value = currentFuel;
            fill.color = gradient.Evaluate(slider.normalizedValue);
            Debug.Log("Current fuel: " + currentFuel);
            alertController.ShowGreenAlert("Fuel tank filled!");
        }
        else
        {
            alertController.ShowRedAlert("Not enough resources!");
        }
    }
}
