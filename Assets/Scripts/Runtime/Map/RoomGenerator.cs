using System.Collections.Generic;
using Random = System.Random;

namespace ProjectGame.DungeonMap
{
    public static class RoomGenerator
    {
        public static void Generate(Map map, List<RoomData> roomTypes, Random rng)
        {
            List<RoomNode> allRooms = GetAllRoomNodes(map);
            
            int leastExecutionOrder = int.MaxValue;
            int previousExecutionOrder;

            foreach (RoomData room in roomTypes)
            {
                foreach (var spawnRule in room.SpawnRules)
                {
                    if (spawnRule.RuleExecutionOrder < leastExecutionOrder)
                        leastExecutionOrder = spawnRule.RuleExecutionOrder;
                }
            }
            
            do
            {
                previousExecutionOrder = leastExecutionOrder;
                leastExecutionOrder = int.MaxValue;
                foreach (RoomData currentData in roomTypes)
                {
                    foreach (BaseSpawnRule spawnRule in currentData.SpawnRules)
                    {
                        if (spawnRule.RuleExecutionOrder < leastExecutionOrder &&
                            spawnRule.RuleExecutionOrder > previousExecutionOrder)
                            leastExecutionOrder = spawnRule.RuleExecutionOrder;
                        
                        if (spawnRule.RuleExecutionOrder != previousExecutionOrder) continue;
            
                        for (int i = allRooms.Count - 1; i >= 0; i--)
                        {
                            if (spawnRule.SpawnInCell(map, allRooms[i], currentData, rng))
                            {
                                allRooms[i].Data = currentData;
                                if (!spawnRule.CanBeOverriden) allRooms.Remove(allRooms[i]);
                            }
                        }
                    }
                }
            } while (previousExecutionOrder != leastExecutionOrder);
        }

        private static List<RoomNode> GetAllRoomNodes(Map map)
        {
            List<RoomNode> roomNodes = new List<RoomNode>();

            foreach (RoomNode roomNode in map)
            {
                if (roomNode.HasConnection)
                    roomNodes.Add(roomNode);
            }

            return roomNodes;
        }
    }
}