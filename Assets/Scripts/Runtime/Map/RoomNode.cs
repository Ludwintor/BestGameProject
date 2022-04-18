using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProjectGame.Characters;
using UnityEngine;

namespace ProjectGame.DungeonMap
{
    public class RoomNode
    {
        public Vector2Int Position { get; }
        public List<RoomNode> ChildrenNodes { get; }
        public List<RoomNode> ParentNodes { get; }
        public RoomData Data { get; set; }
        public bool HasConnection => ChildrenNodes.Count > 0 || ParentNodes.Count > 0;
        public ReadOnlyCollection<Enemy> Enemies => _enemies.AsReadOnly();

        private List<Enemy> _enemies;

        public RoomNode(int x, int y)
        {
            Position = new Vector2Int(x, y);
            ChildrenNodes = new List<RoomNode>();
            ParentNodes = new List<RoomNode>();
        }

        public RoomNode GetMaxChild() => GetMax(ChildrenNodes);

        public RoomNode GetMinChild() => GetMin(ChildrenNodes);

        public RoomNode GetMaxParent() => GetMax(ParentNodes);
        public RoomNode GetMinParent() => GetMin(ParentNodes);

        private RoomNode GetMax(List<RoomNode> nodes)
        {
            if (nodes.Count == 0)
                return null;
            RoomNode max = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                RoomNode current = nodes[i];
                if (current.Position.x > max.Position.x)
                    max = current;
            }
            return max;
        }

        private RoomNode GetMin(List<RoomNode> nodes)
        {
            if (nodes.Count == 0)
                return null;
            RoomNode min = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                RoomNode current = nodes[i];
                if (current.Position.x < min.Position.x)
                    min = current;
            }
            return min;
        }

        public RoomNode GetCommonAncestor(RoomNode other, int maxDepth)
        {
            Debug.Assert(Position.y == other.Position.y, "To find an ancestor, nodes must be on same y");
            RoomNode leftNode = Position.x < other.Position.x ? this : other;
            RoomNode rightNode = Position.x < other.Position.x ? other : this;
            int lastY = Position.y - maxDepth;
            for (int currentY = Position.y; currentY >= lastY; currentY--)
            {
                if (leftNode.ParentNodes.Count == 0 || rightNode.ParentNodes.Count == 0)
                    return null;
                leftNode = leftNode.GetMaxParent();
                rightNode = rightNode.GetMinParent();
                if (leftNode == rightNode)
                    return leftNode;
            }
            return null;
        }
    }
}
