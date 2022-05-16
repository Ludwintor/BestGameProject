using System.Collections.Generic;

namespace ProjectGame.DungeonMap
{
    public static class RoomGenerator
    {
        public static void Generate(Map map, RoomData[] roomTypes, SpawnRule[] spawnRules, RNG rng)
        {
            List<RoomNode> allRooms = GetAllRoomNodes(map);

            foreach (SpawnRule spawnRule in spawnRules)
            {
                foreach (RoomType roomType in spawnRule.GetRoomTypes())
                {
                    for (int i = allRooms.Count - 1; i >= 0; i--)
                    {
                        if (spawnRule.SpawnInCell(allRooms[i], roomType, rng))
                        {
                            allRooms[i].RoomType = roomType;
                            allRooms[i].Data = FindDataWithType(roomTypes, roomType);
                            if (!spawnRule.CanBeOverriden)
                            {
                                allRooms.Remove(allRooms[i]);
                                
                            }
                        }
                    }   
                }
            }
        }

        private static RoomData FindDataWithType(RoomData[] rooms, RoomType type)
        {
            foreach (RoomData room in rooms)
            {
                if (room.roomType == type)
                {
                    return room;
                }
            }
            return null;
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