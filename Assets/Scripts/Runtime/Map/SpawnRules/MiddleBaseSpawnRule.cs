using ProjectGame.DungeonMap;
using UnityEngine;

namespace ProjectGame
{
    [CreateAssetMenu(menuName = "Game/Rooms/SpawnRules/Middle")]
    public class MiddleBaseSpawnRule : BaseSpawnRule
    {
        public override bool CanSpawnInCell(Map map, RoomNode room, RoomData roomData)
        {
            return room.Position.y == map.Rows / 2;
        }
    }
}
