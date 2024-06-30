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

    public float temporaryVisibilityRadius = 10f;
    public float visibilityDuration = 3f;

    private Coroutine visibilityCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        directionChangeTimer = changeDirectionInterval;
        ChangeDirection();

        GameObject visibilityControllerObject = GameObject.Find("VisibilityController");
        if (visibilityControllerObject != null)
        {
            visibilityController = visibilityControllerObject.GetComponent<VisibilityController>();
        }
        else
        {
            Debug.LogError("VisibilityController object not found in the scene.");
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

    void OnDisable()
    {
        if (visibilityController != null && visibilityCoroutine == null)
        {
            // visibilityCoroutine = StartCoroutine(TemporaryVisibility());
        }
    }

    private IEnumerator TemporaryVisibility()
    {
        float originalRadius = visibilityController.playerVisibility.visibilityRadius;
        visibilityController.SetNewVisibilityRadius(temporaryVisibilityRadius);
        yield return new WaitForSeconds(visibilityDuration);
        visibilityController.SetNewVisibilityRadius(originalRadius);
        visibilityCoroutine = null;
    }
}
