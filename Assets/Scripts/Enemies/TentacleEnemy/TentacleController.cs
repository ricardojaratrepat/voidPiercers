using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;


namespace Tentacle
{
    public class TentacleController : MonoBehaviour
    {
        // Start is called before the first frame update

        private readonly float shootPeriod = 2.0f;
        private float elapsedTime = 0.0f;
        private bool isShooting = false;
        private GameObject player;
        private Animator animator;
        private EnemyMovementRangedAI enemyMovementRangedAI;

        public GameObject acidBallPrefab;

        void Start()
        {
            player = FindObjectOfType<PlayerController>().gameObject;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 10)
            {
                isShooting = true;
                animator.SetBool("attackingPlayer", true);
                

                if (player.transform.position.x < transform.position.x)
                {
                    Vector3 newScale = transform.localScale;
                    newScale.x = -1;
                    transform.localScale = newScale;
                }
                else
                {
                    Vector3 newScale = transform.localScale;
                    newScale.x = 1;
                    transform.localScale = newScale;
                }
            }
            else
            {
                isShooting = false;
                animator.SetBool("attackingPlayer", false);
            }

            if (isShooting)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= shootPeriod)
                {
                    Shoot();
                    elapsedTime = 0.0f;
                }
            }
        }

        private void Shoot()
        {
            GameObject acidBall = Instantiate(acidBallPrefab, transform.position, Quaternion.identity);
        }
    }

}
