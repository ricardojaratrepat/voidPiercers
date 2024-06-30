using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    public class EnemyMovementRangedAI : MonoBehaviour
    {
        public Transform player;
        public float minDistanceToActivatePathfinding = 5f; // Distancia mínima para activar el cálculo de ruta
        public float maxDistanceFromPlayer = 10f; // Distancia máxima para encontrar una posición válida
        public float velocity = 5f; // Velocidad de movimiento del enemigo
        public float periodPerRecalculation = 1f; // Periodo de tiempo para recalcular la ruta
        float elapsedTime = 0f; // Tiempo transcurrido desde el último cálculo de ruta
        GridManager gridManager;
        Pathfinding pathfinding;
        List<Node> path;
        int currentPathIndex = 0; // Índice del nodo actual en la ruta


        void Start()
        {
            if (!player) player = GameObject.FindWithTag("Player").transform;
            pathfinding = FindObjectOfType<Pathfinding>();
            gridManager = pathfinding.GetComponent<GridManager>();
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

                Vector2 enemyPos = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

                List<Vector2Int> possibleNodes = gridManager.GetNodesWithinDistance(new Vector2Int((int)enemyPos.x, (int)enemyPos.y), (int)maxDistanceFromPlayer);

                Vector2Int targetNode = Vector2Int.zero;
                float closestDistance = Mathf.Infinity;

                foreach (Vector2Int node in possibleNodes)
                {
                    Vector2 nodeWorldPos = gridManager.GridToWorldPosition(node);
                    Vector2 direction = (Vector2)player.position - nodeWorldPos;

                    int layerMask = ~(1 << LayerMask.NameToLayer("Enemy"));

                    RaycastHit2D hit = Physics2D.Raycast(nodeWorldPos, direction, Mathf.Infinity, layerMask);

                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        float distance = Vector2.Distance(enemyPos, node);

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            targetNode = node;
                        }
                    }
                }

                if (targetNode != Vector2Int.zero)
                {
                    path = pathfinding.FindPath(enemyPos, new Vector2(targetNode.x, targetNode.y));
                    currentPathIndex = 0;
                }
            }

            if (path != null && path.Count > 0 && currentPathIndex < path.Count)
            {
                Vector3 nextPos = new Vector3(path[currentPathIndex].Position.x, path[currentPathIndex].Position.y, 0);
                transform.position = Vector3.MoveTowards(transform.position, nextPos, velocity * Time.deltaTime);

                if (Vector3.Distance(transform.position, nextPos) < 0.1f)
                {
                    currentPathIndex++;
                    if (currentPathIndex >= path.Count)
                    {
                        path = null;
                    }
                }
            }
        }
    }
}
