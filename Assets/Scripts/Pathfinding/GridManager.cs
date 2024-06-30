using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    public class GridManager : MonoBehaviour
    {
        public int width;
        public int height;
        public Node[,] grid;
        public GameObject redDotPrefab;
        
        // Intervalo de tiempo para la actualización del grid
        public float updateInterval = 3f;
        private float timer = 0f;
        private readonly bool visualizeGrid = false;

        void Start()
        {
            CreateGrid();
        }

        void Update()
        {
            // Contador para la actualización periódica del grid
            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                UpdateGrid();
                timer = 0f;
            }
        }

        void CreateGrid()
        {
            grid = new Node[Mathf.FloorToInt(width) + 1 , Mathf.FloorToInt(height) + 1];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Vector2 position = GridToWorldPosition(new Vector2Int(x, y));
                    bool isWalkable = IsPositionWalkable(position);
                    grid[x, y] = new Node(position, isWalkable);

                    if (visualizeGrid)
                    {
                        GameObject redDot = Instantiate(redDotPrefab, position, Quaternion.identity);
                        redDot.transform.parent = transform;
                    }   

                }
            }
        }

        void UpdateGrid()
        {
            Debug.Log("Updating grid...");
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Vector2 position = GridToWorldPosition(new Vector2Int(x, y));
                    bool isWalkable = IsPositionWalkable(position);
                    grid[x, y].IsWalkable = isWalkable;
                }
            }
        }

        bool IsPositionWalkable(Vector2 position)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(position);
            
            foreach (Collider2D collider in colliders)
            {
                // Aquí defines qué tags corresponden a objetos que bloquean el paso
                if (collider.CompareTag("Ground"))
                {
                    return false;
                }
            }
            
            return true;
        }

        public void UpdateNode(Vector2Int gridPosition, bool isWalkable)
        {
            if (gridPosition.x >= 0 && gridPosition.x < grid.GetLength(0) && gridPosition.y >= 0 && gridPosition.y < grid.GetLength(1))
            {
                grid[gridPosition.x, gridPosition.y].IsWalkable = isWalkable;
            }
        }

        public Vector2 GridToWorldPosition(Vector2Int gridPosition)
        {
            // float x = gridPosition.x + 0.5f;
            // float y = gridPosition.y + 0.5f;
            float x = gridPosition.x;
            float y = gridPosition.y;
            return new Vector2(x, y);
        }

        public List<Vector2Int> GetNodesWithinDistance(Vector2Int startNode, int r)
        {
            List<Vector2Int> result = new List<Vector2Int>();
            int startX = startNode.x;
            int startY = startNode.y;

            for (int x = Mathf.Max(0, startX - r); x <= Mathf.Min(width - 1, startX + r); x++)
            {
                for (int y = Mathf.Max(0, startY - r); y <= Mathf.Min(height - 1, startY + r); y++)
                {
                    if (!grid[x, y].IsWalkable) continue;
                    
                    if (Mathf.Sqrt((x - startX) * (x - startX) + (y - startY) * (y - startY)) <= r)
                    {
                        result.Add(new Vector2Int(x, y));
                    }
                }
            }

            return result;
        }
    }
}
