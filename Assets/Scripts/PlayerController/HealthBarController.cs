using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHealth = 100f;
    public float currentHealth;
    private Slider slider;
    public Gradient gradient;
    public Image fill;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        currentHealth = maxHealth;
        
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

    public void StartContinuousDamage(float damagePerSecond, float duration)
    {
        StartCoroutine(ApplyDamageOverTime(damagePerSecond, duration));
    }

    private IEnumerator ApplyDamageOverTime(float damagePerSecond, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            TakeDamage(damagePerSecond);
            elapsed += 1f;
            yield return new WaitForSeconds(1f);
        }
    }

    private void Die()
    {
        Debug.Log("Player died");
        StartCoroutine(RestartLevel());
    }

    private IEnumerator RestartLevel()
    {
        // Espera un segundo antes de recargar la escena (opcional)
        yield return new WaitForSeconds(1f);
        
        // Recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
