using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        }
        slider.value = currentHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
