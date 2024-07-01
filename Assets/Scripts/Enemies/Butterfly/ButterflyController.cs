using System.Collections;
using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 velocity = Vector2.zero;

    public float flightSpeed = 2f;
    public float changeDirectionInterval = 3f;

    private float directionChangeTimer;
    private VisibilityController visibilityController;

    public float visibilityDuration = 3f;

    private Coroutine visibilityCoroutine;
    private HealthController healthController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        directionChangeTimer = changeDirectionInterval;
        healthController = FindObjectOfType<HealthController>();
        ChangeDirection();

        visibilityController = FindObjectOfType<VisibilityController>();
    }

    void Update()
    {
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0)
        {
            ChangeDirection();
            directionChangeTimer = changeDirectionInterval;
        }

        if (velocity.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    void ChangeDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        velocity = direction * flightSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        velocity = -velocity;
    }

    void OnDestroy()
    {
        healthController.currentHealth += 15;
        healthController.slider.value = healthController.currentHealth;
        healthController.fill.color = healthController.gradient.Evaluate(healthController.slider.normalizedValue);
    }
}
