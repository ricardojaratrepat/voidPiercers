using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mushroom
{
    public class MushroomController : MonoBehaviour
    {
        private GameObject Player;
        private Rigidbody2D rb;
        private Animator animator;
        private Vector2 velocity;
        private readonly float damage = 40f;

        private readonly float chaseSpeed = 5f;
        private readonly float minChaseDistance = 15f;
        public float jumpForce = 1f; // Fuerza del salto

        private readonly float explotionRadius = 3f;
        private float distanceToPlayer;
        private float localScaleFactor;
        private CapsuleCollider2D bodyCollider;

        public float jumpCooldown = 1f; // Tiempo de enfriamiento entre saltos
        private float timeSinceLastJump = 0f;

        void Start()
        {
            Player = FindObjectOfType<PlayerController>()?.gameObject;
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            bodyCollider = GetComponent<CapsuleCollider2D>();
            localScaleFactor = transform.localScale.x;
        }

        // Update is called once per frame
        void Update()
        {
            if (Player == null) return;

            distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
            timeSinceLastJump += Time.deltaTime;

            if (distanceToPlayer < minChaseDistance)
            {
                Vector2 directionToPlayer = (Player.transform.position - transform.position).normalized;

                velocity.x = directionToPlayer.x * chaseSpeed;
                animator.SetBool("chasingPlayer", true);

                if (Player.transform.position.x < transform.position.x)
                {
                    Vector3 newScale = transform.localScale;
                    newScale.x = -localScaleFactor;
                    transform.localScale = newScale;
                }
                else
                {
                    Vector3 newScale = transform.localScale;
                    newScale.x = localScaleFactor;
                    transform.localScale = newScale;
                }

                // Raycast para detectar obstáculos
                Vector3 raycastPosition = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                Vector2 raycastDirection = new Vector2(transform.localScale.x, 0); // Dirección horizontal

                // Crear una máscara que excluya la capa "IgnoreRayCast" y "Enemy"
                int layerMask = ~LayerMask.GetMask("Ignore RayCast", "Enemy");

                RaycastHit2D hit = Physics2D.Raycast(raycastPosition, raycastDirection, 2f, layerMask);
                
    

                if (hit.collider != null && timeSinceLastJump >= jumpCooldown)
                {
                    Vector2 forceDirection = new Vector2(-transform.localScale.x, 1); // Dirección vertical
                    Debug.Log(forceDirection);
                    rb.AddForce(forceDirection * jumpForce, ForceMode2D.Impulse);
                    timeSinceLastJump = 0f; // Reinicia el temporizador
                }

            }
            else
            {
                velocity.x = 0f;
                animator.SetBool("chasingPlayer", false);
            }
        }

        void FixedUpdate()
        {
            if (Player == null) return;

            Vector2 newVelocity = rb.velocity;
            newVelocity.x = velocity.x;
            rb.velocity = newVelocity;

        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {   
                animator.SetBool("touchPlayer", true);
                bodyCollider.enabled = false;
                rb.gravityScale = 0;
            }
        }

        public void DestroyMushroom()
        {
            Destroy(gameObject);
        }

        public void ProduceDamage()
        {
            if (Player == null) return;

            if (distanceToPlayer < explotionRadius)
            {
                HealthController healthController = Player.GetComponent<HealthController>();

                if (healthController != null)
                {
                    healthController.TakeDamage(damage);
                }
                else
                {
                    Debug.LogError("HealthController not found on Player!");
                }
            }
        }
    }
}
