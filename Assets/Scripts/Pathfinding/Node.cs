using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Node
    {
        public Vector2 position;
        public bool isWalkable;
        public int gCost;
        public int hCost;
        public Node parent;

        public Node(Vector2 _position, bool _isWalkable)
        {
            position = _position;
            isWalkable = _isWalkable;
        }

        public int FCost
        {
            get { return gCost + hCost; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public bool IsWalkable
        {
            get { return isWalkable; }
            set { isWalkable = value; }
        }

        public int GCost
        {
            get { return gCost; }
            set { gCost = value; }
        }

        public int HCost
        {
            get { return hCost; }
            set { hCost = value; }
        }

        public Node Parent
        {
            get { return parent; }
            set { parent = value; }
        }
    }
}

