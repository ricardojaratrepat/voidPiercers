using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private GameOverManager gameOverManager; // Referencia al GameOverManager

    public AudioClip hitSound; // Variable para el sonido de golpe
    public AudioSource hitAudioSource; // Referencia explícita al AudioSource del sonido de golpe

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

        if (hitAudioSource == null)
        {
            Debug.LogError("HitAudioSource is not assigned in the inspector.");
        }

        if (hitSound == null)
        {
            Debug.LogError("HitSound is not assigned in the HealthController script.");
        }
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

        PlayHitSound(); // Reproducir el sonido de golpe cuando se recibe daño
    }

    public void Die()
    {
        gameOverManager.GameOver(); // Mostrar la pantalla de Game Over
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

    private void PlayHitSound()
    {
        if (hitAudioSource != null && hitSound != null)
        {
            hitAudioSource.PlayOneShot(hitSound);
            Debug.Log("Playing hit sound");
        }
        else
        {
            Debug.LogWarning("HitAudioSource or HitSound is not assigned.");
        }
    }
}
