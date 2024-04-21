using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHealth = 100f;
    public float currentHealth;

    private GameObject healthBar;
    private Slider slider;
    public Gradient gradient;
    public Image fill;

    void Start()
    {
        healthBar = GameObject.Find("HealthBar");
        slider = healthBar.GetComponent<Slider>();
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        currentHealth = maxHealth;
        
        fill.color = gradient.Evaluate(1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        healthBar.GetComponent<Slider>().value = currentHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
