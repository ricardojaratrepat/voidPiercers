using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    private GameObject Player;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 velocity = Vector2.zero;

    public float chaseSpeed = 5f;
    public float minChaseDistance = 15f;

    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f; // Duración del knockback en segundos

    private bool isKnockbackActive = false; // Variable para controlar si el knockback está activo

    private float damage = 10f;

    void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);

        if (distanceToPlayer < minChaseDistance)
        {
            Vector2 directionToPlayer = (Player.transform.position - transform.position).normalized;

            velocity = directionToPlayer * chaseSpeed;
            animator.SetBool("chasingPlayer", true);

            if (Player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            velocity = Vector2.zero;
            animator.SetBool("chasingPlayer", false);
        }
    }

    void FixedUpdate()
    {
        if (!isKnockbackActive)
        {
            rb.velocity = velocity;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Player)
        {
            // // Player.GetComponent<HealthController>().TakeDamage(damage);
            FindFirstObjectByType<HealthController>().TakeDamage(damage);
            // Calcular dirección de knockback
            Vector2 knockbackDirection = transform.position - Player.transform.position;
            knockbackDirection.Normalize(); // Normalizar para una fuerza consistente

            // Aplicar impulso de knockback
            ApplyKnockback(knockbackDirection);
        }
    }

    // Función para aplicar el knockback
    void ApplyKnockback(Vector2 knockbackDirection)
    {
        if (!isKnockbackActive)
        {
            isKnockbackActive = true;
            rb.velocity = knockbackDirection * knockbackForce;

            // Iniciar temporizador para desactivar el knockback después de cierto tiempo
            StartCoroutine(DisableKnockbackAfterDelay());
        }
    }

    // Coroutine para desactivar el knockback después de cierto tiempo
    IEnumerator DisableKnockbackAfterDelay()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockbackActive = false;
        rb.velocity = Vector2.zero; // Reiniciar la velocidad después del knockback
    }
}
