using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 velocity = Vector2.zero;
    public GameObject darkCircle;

    public float flightSpeed = 2f; // Velocidad de vuelo de la mariposa
    public float changeDirectionInterval = 3f; // Intervalo de tiempo para cambiar de dirección

    private float directionChangeTimer;
    private DarkCircleController darkCircleController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        directionChangeTimer = changeDirectionInterval;
        ChangeDirection();
        
        // Obtener el componente DarkCircleController del darkCircle
        darkCircleController = darkCircle.GetComponent<DarkCircleController>();
        if (darkCircleController == null)
        {
            Debug.LogError("No se encontró el componente DarkCircleController en el darkCircle.");
        }
    }

    void Update()
    {
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0)
        {
            ChangeDirection();
            directionChangeTimer = changeDirectionInterval;
        }

        animator.SetBool("flying", true);

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

    void OnDestroy()
    {
        if (darkCircleController != null)
        {
            darkCircleController.EnlargeDarkCircle();
        }
    }

    void ChangeDirection()
    {
        // Generar una dirección aleatoria
        float randomAngle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        velocity = direction * flightSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Invertir la dirección de la mariposa al chocar con cualquier Rigidbody2D
        velocity = -velocity;
    }
}
