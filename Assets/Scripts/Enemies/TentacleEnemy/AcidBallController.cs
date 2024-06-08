using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcidBall
{
    public class AcidBallController : MonoBehaviour
    {
        private readonly float damageDuration = 5.0f;
        private readonly float damagePerSecond = 3;
        private readonly float acidBallSpeed = 5.0f;

        private GameObject player;
        private Animator animator;
        private Rigidbody2D rb;
        private CircleCollider2D circleCollider;

        void Start()
        {
            player = FindObjectOfType<PlayerController>().gameObject;
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            circleCollider = GetComponent<CircleCollider2D>();

            rb.velocity = (player.transform.position - transform.position).normalized * acidBallSpeed;

        }

        void Update()
        {

        }


        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == player)
            {
                circleCollider.enabled = false;
                animator.SetBool("touchPlayer", true);
                rb.velocity = Vector2.zero;
                FindFirstObjectByType<HealthController>().StartContinuousDamage(damagePerSecond, damageDuration);
            }
        }

        public void DestroyAcidBall()
        {
            Destroy(gameObject);
        }


    }
}
