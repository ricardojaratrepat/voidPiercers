using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    public class GridManager : MonoBehaviour
    {
        public int width;
        public int height;
        public float density = 1f; // Densidad de la cuadrícula
        public Node[,] grid;

        // Intervalo de tiempo para la actualización del grid
        public float updateInterval = 3f;
        private float timer = 0f;

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
            grid = new Node[Mathf.FloorToInt(width * density), Mathf.FloorToInt(height * density)];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Vector2 position = GridToWorldPosition(new Vector2Int(x, y));
                    bool isWalkable = IsPositionWalkable(position);
                    grid[x, y] = new Node(new Vector2Int(x, y), isWalkable);
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
            float x = gridPosition.x / density;
            float y = gridPosition.y / density;
            return new Vector2(x, y);
        }

        public Vector2Int WorldToGridPosition(Vector2 worldPosition)
        {
            int x = Mathf.FloorToInt(worldPosition.x * density);
            int y = Mathf.FloorToInt(worldPosition.y * density);
            return new Vector2Int(x, y);
        }
    }
}
