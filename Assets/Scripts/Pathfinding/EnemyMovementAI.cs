using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    public class EnemyMovement : MonoBehaviour
    {
        public Transform player;
        public float minDistanceToActivatePathfinding = 5f; // Distancia mínima para activar el cálculo de ruta
        public float velocity = 5f; // Velocidad de movimiento del enemigo
        public float periodPerRecalculation = 1f; // Periodo de tiempo para recalcular la ruta
        float elapsedTime = 0f; // Tiempo transcurrido desde el último cálculo de ruta
        Pathfinding pathfinding;
        List<Node> path;
        int currentPathIndex = 0; // Índice del nodo actual en la ruta

        void Start()
        {
            pathfinding = FindObjectOfType<Pathfinding>();
        }

        void Update()
        {
            if (!player) return;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer > minDistanceToActivatePathfinding) return;

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= periodPerRecalculation || path == null || path.Count == 0)
            {
                elapsedTime = 0f;

                // Obtener las posiciones redondeadas del enemigo y del jugador
                Vector2 enemyPos = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                Vector2 playerPos = new Vector2(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y));

                // Calcular la ruta usando el Pathfinding
                string pathStr = "";

                path = pathfinding.FindPath(enemyPos, playerPos);

                Debug.Log("Path:");
                path.ForEach(node => pathStr += node.Position + " ");
                Debug.Log(pathStr);

                currentPathIndex = 0; // Reiniciar el índice del camino
            }

            if (path != null && path.Count > 0 && currentPathIndex < path.Count)
            {
                // Mover hacia el siguiente nodo en la ruta con la velocidad especificada
                Vector3 nextPos = new Vector3(path[currentPathIndex].Position.x, path[currentPathIndex].Position.y, 0);
                transform.position = Vector3.MoveTowards(transform.position, nextPos, velocity * Time.deltaTime);

                // Si el enemigo ha llegado al nodo actual, avanzar al siguiente nodo en la ruta
                if (Vector3.Distance(transform.position, nextPos) < 0.1f)
                {
                    currentPathIndex++;
                    // Verificar si se ha alcanzado el final de la ruta
                    if (currentPathIndex >= path.Count)
                    {
                        path = null; // Limpiar la ruta una vez que se ha alcanzado el objetivo
                    }
                }
            }
        }
    }
}
