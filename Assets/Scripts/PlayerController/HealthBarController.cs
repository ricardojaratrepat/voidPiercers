using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private GameOverManager gameOverManager; // Referencia al GameOverManager

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        currentHealth = maxHealth;

        gameOverManager = FindObjectOfType<GameOverManager>(); // Inicializa la referencia
        fill.color = gradient.Evaluate(1f);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        slider.value = currentHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private void Die()
    {
        gameOverManager.GameOver(); // Mostrar la pantalla de Game Over
    }
}
