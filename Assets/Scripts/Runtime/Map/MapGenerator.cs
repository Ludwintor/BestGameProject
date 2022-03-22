using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace ProjectGame.DungeonMap
{
    public static class MapGenerator
    {
        /// <summary>
        /// Generate map with given rows and columns.
        /// Consider using odd number for columns
        /// </summary>
        /// <param name="density">Iterations count for constructing path from starting node to last</param>
        /// <param name="maxStart">Max randomized starting nodes. Still could be less</param>
        public static Map GenerateMap(int rows, int columns, int density, int maxStart, int maxAncestorDepth, Random rng)
        {
            Map map = new Map(rows, columns);
            List<RoomNode> startingNodes = new List<RoomNode>(maxStart);
            for (int i = 0; i < density; i++)
            {
                RoomNode startingNode = GetStartingNode(map, startingNodes, maxStart, rng);
                CreatePath(map, startingNode, maxAncestorDepth, rng);
            }
            return map;
        }

        private static RoomNode GetStartingNode(Map map, List<RoomNode> startingNodes, int maxStart, Random rng)
        {
            RoomNode startingNode;
            if (startingNodes.Count < maxStart)
            {
                int randomStartingIndex = rng.Next(map.Columns);
                startingNode = map.GetNode(randomStartingIndex, 0);
                if (!startingNodes.Contains(startingNode))
                    startingNodes.Add(startingNode);
            }
            else
            {
                int randomStartingIndex = rng.Next(startingNodes.Count);
                startingNode = startingNodes[randomStartingIndex];
            }
            return startingNode;
        }

        private static void CreatePath(Map map, RoomNode node, int maxAncestorDepth, Random rng)
        {
            if (node.Position.y + 2 >= map.Rows)
            {
                RoomNode lastNode = map.GetNode(map.Columns / 2, map.Rows - 1);
                node.ChildrenNodes.Add(lastNode);
                lastNode.ParentNodes.Add(node);
                return;
            }

            int rowSize = map.Columns - 1;
            int nextX = node.Position.x + rng.Next(-1, 1 + 1);
            int nextY = node.Position.y + 1;
            nextX = Mathf.Clamp(nextX, 0, rowSize);

            foreach (RoomNode targetParent in map.GetNode(nextX, nextY).ParentNodes)
            {
                if (targetParent == node)
                    continue;
                RoomNode ancestor = node.GetCommonAncestor(targetParent, maxAncestorDepth);
                if (ancestor != null)
                {
                    if (nextX > node.Position.x)
                        nextX = node.Position.x + rng.Next(-1, 0 + 1);
                    else if (nextX == node.Position.x)
                        nextX = node.Position.x + rng.Next(-1, 1 + 1);
                    else
                        nextX = node.Position.x + rng.Next(0, 1 + 1);
                    nextX = Mathf.Clamp(nextX, 0, rowSize);
                    break;
                }
            }

            // Check if two connections are intersect and fix this
            if (node.Position.x > 0)
            {
                RoomNode leftNode = map.GetNode(node.Position.x - 1, node.Position.y);
                RoomNode rightChildOfLeft = leftNode.GetMaxChild();
                // Left node has child and target path intersects with existing path
                if (rightChildOfLeft != null && rightChildOfLeft.Position.x > nextX)
                    nextX = rightChildOfLeft.Position.x;
            }
            if (node.Position.x < rowSize)
            {
                RoomNode rightNode = map.GetNode(node.Position.x + 1, node.Position.y);
                RoomNode leftChildOfRight = rightNode.GetMinChild();
                // Right node has child and target path intersects with existing path
                if (leftChildOfRight != null && leftChildOfRight.Position.x < nextX)
                    nextX = leftChildOfRight.Position.x;
            }
            RoomNode targetNode = map.GetNode(nextX, nextY);
            if (!node.ChildrenNodes.Contains(targetNode))
                node.ChildrenNodes.Add(targetNode);
            if (!targetNode.ParentNodes.Contains(node))
                targetNode.ParentNodes.Add(node);
            CreatePath(map, targetNode, maxAncestorDepth, rng);
        }
    }
}
