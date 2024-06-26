using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Pathfinding : MonoBehaviour
    {
        public GridManager gridManager;

        void Start()
        {
            // Aquí podrías agregar inicialización adicional si es necesaria
            // Por ejemplo, podrías obtener una referencia a GridManager si no está asignada directamente en el Inspector.
            if (gridManager == null)
            {
                gridManager = GetComponent<GridManager>();
                if (gridManager == null)
                {
                    Debug.LogError("GridManager no encontrado en el GameObject o no asignado manualmente.");
                }
            }
        }

        public List<Node> FindPath(Vector2 startPos, Vector2 endPos)
        {
            Node startNode = GetNodeFromPosition(startPos);
            Node endNode = GetNodeFromPosition(endPos);

            if (startNode == null || endNode == null)
            {
                Debug.LogWarning("No se pudo encontrar el nodo de inicio o el nodo de destino.");
                return null;
            }

            List<Node> openList = new List<Node>();
            HashSet<Node> closedList = new HashSet<Node>();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList[0];

                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (currentNode == endNode)
                {
                    return RetracePath(startNode, endNode);
                }

                foreach (Node neighbor in GetNeighbors(currentNode))
                {
                    if (!neighbor.IsWalkable || closedList.Contains(neighbor))
                    {
                        continue;
                    }

                    int newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                    if (newCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor))
                    {
                        neighbor.GCost = newCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, endNode);
                        neighbor.Parent = currentNode;

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = Mathf.FloorToInt(node.Position.x) + x;
                    int checkY = Mathf.FloorToInt(node.Position.y) + y;

                    if (checkX >= 0 && checkX < gridManager.grid.GetLength(0) && checkY >= 0 && checkY < gridManager.grid.GetLength(1))
                    {
                        neighbors.Add(gridManager.grid[checkX, checkY]);
                    }
                }
            }

            return neighbors;
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(Mathf.FloorToInt(nodeA.Position.x) - Mathf.FloorToInt(nodeB.Position.x));
            int dstY = Mathf.Abs(Mathf.FloorToInt(nodeA.Position.y) - Mathf.FloorToInt(nodeB.Position.y));

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }
            return 14 * dstX + 10 * (dstY - dstX);
        }

        List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }

        Node GetNodeFromPosition(Vector2 position)
        {
            try
            {
                int x = Mathf.FloorToInt(position.x * gridManager.density);
                int y = Mathf.FloorToInt(position.y * gridManager.density);
                return gridManager.grid[x, y];
            }
            catch
            {
                return null;
            }
        }
    }
}
