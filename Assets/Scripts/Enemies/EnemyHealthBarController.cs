using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarController : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private EnemyHealth enemyHealth;
    private GameObject enemyObject;
    private Canvas healthBarCanvas;

    void Start()
    {
        enemyObject = transform.parent.parent.gameObject;
        enemyHealth = enemyObject.GetComponent<EnemyHealth>();
        slider = GetComponent<Slider>();
        healthBarCanvas = GetComponentInParent<Canvas>(); // Obtener el Canvas del slider

        // Inicializar el valor del slider
        slider.maxValue = enemyHealth.maxHealth;
        slider.value = enemyHealth.currentHealth;

        // Inicializar la visibilidad del canvas
        UpdateHealthBarVisibility();
    }

    void Update()
    {
        slider.value = enemyHealth.currentHealth;

        // Ajustar la escala para evitar invertir el slider
        if (enemyObject.transform.localScale.x < 0)
        {
            slider.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            slider.transform.localScale = new Vector3(1, 1, 1);
        }

        // Actualizar la visibilidad de la barra de salud
        UpdateHealthBarVisibility();
    }

    void UpdateHealthBarVisibility()
    {
        // Hacer que el Canvas sea visible sólo si el slider no está lleno
        if (slider.value == slider.maxValue)
        {
            healthBarCanvas.enabled = false;
        }
        else
        {
            healthBarCanvas.enabled = true;
        }
    }
}
