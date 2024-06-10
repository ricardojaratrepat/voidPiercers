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
    public float knockbackDuration = 0.5f; 

    private bool isKnockbackActive = false; 

    private float damage = 10f;

    void Start()
    {
        Player = FindObjectOfType<PlayerController>().gameObject;
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
            Player.GetComponent<HealthController>().TakeDamage(damage); // Cambiado para usar GetComponent

            Vector2 knockbackDirection = transform.position - Player.transform.position;
            knockbackDirection.Normalize();

            ApplyKnockback(knockbackDirection);
        }
    }

    void ApplyKnockback(Vector2 knockbackDirection)
    {
        if (!isKnockbackActive)
        {
            isKnockbackActive = true;
            rb.velocity = knockbackDirection * knockbackForce;

            StartCoroutine(DisableKnockbackAfterDelay());
        }
    }

    IEnumerator DisableKnockbackAfterDelay()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockbackActive = false;
        rb.velocity = Vector2.zero;
    }
}
