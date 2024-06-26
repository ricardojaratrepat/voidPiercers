using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    public class EnemyMovement : MonoBehaviour
    {
        public Transform player;
        public float minDistanceToActivatePathfinding = 5f; // Distancia mínima para activar el cálculo de ruta
        public float velocity = 5f; // Velocidad de movimiento del enemigo
        Pathfinding pathfinding;
        List<Node> path;
        int currentPathIndex = 0; // Índice del nodo actual en la ruta

        void Start()
        {
            pathfinding = FindObjectOfType<Pathfinding>();
        }

        void Update()
        {
            if (player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);

                // Activar el cálculo de la ruta solo si la distancia al jugador es menor que la distancia mínima especificada
                if (distanceToPlayer < minDistanceToActivatePathfinding)
                {
                    // Obtener las posiciones redondeadas del enemigo y del jugador
                    Vector2 enemyPos = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                    Vector2 playerPos = new Vector2(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y));

                    // Calcular la ruta usando el Pathfinding
                    path = pathfinding.FindPath(enemyPos, playerPos);

                    if (path != null && path.Count > 0)
                    {
                        // Mover hacia el siguiente nodo en la ruta con la velocidad especificada
                        Vector3 nextPos = new Vector3(path[currentPathIndex].Position.x, path[currentPathIndex].Position.y, 0);
                        transform.position = Vector3.MoveTowards(transform.position, nextPos, velocity * Time.deltaTime);

                        // Si el enemigo ha llegado al nodo actual, avanzar al siguiente nodo en la ruta
                        if (transform.position == nextPos)
                        {
                            currentPathIndex++;
                            // Verificar si se ha alcanzado el final de la ruta
                            if (currentPathIndex >= path.Count)
                            {
                                path = null; // Limpiar la ruta una vez que se ha alcanzado el objetivo
                                currentPathIndex = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}
