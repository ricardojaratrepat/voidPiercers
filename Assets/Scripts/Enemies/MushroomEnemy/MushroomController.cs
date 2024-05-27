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

        private readonly float explotionRadius = 3f;
        private float distanceToPlayer;
        private float localScaleFactor;
        private CapsuleCollider2D capsuleCollider;

        void Start()
        {
            Player = FindObjectOfType<PlayerController>().gameObject;
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider2D>();
            localScaleFactor = transform.localScale.x;
        }

        // Update is called once per frame
        void Update()
        {
            distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);

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
            }
            else
            {
                velocity.x = 0f;
                animator.SetBool("chasingPlayer", false);
            }


        }

        void FixedUpdate()
        {
            Vector2 newVelocity = rb.velocity;
            newVelocity.x = velocity.x;
            rb.velocity = newVelocity;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {   
                animator.SetBool("touchPlayer", true);
                capsuleCollider.enabled = false;
                rb.gravityScale = 0;
            }
        }

        public void DestroyMushroom()
        {
            Destroy(gameObject);
        }

        public void ProduceDamage()
        {
             if (distanceToPlayer < explotionRadius)
            {
                FindFirstObjectByType<HealthController>().TakeDamage(damage);
            }
        }
    }
}
