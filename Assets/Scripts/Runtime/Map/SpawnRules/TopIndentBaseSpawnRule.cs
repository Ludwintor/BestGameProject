using ProjectGame.DungeonMap;
using UnityEngine;

namespace ProjectGame
{
    [CreateAssetMenu(menuName = "Game/Rooms/SpawnRules/TopIndent")]
    public class TopIndentBaseSpawnRule : BaseSpawnRule
    {
        public int TopIndent => _topIndent;
        [SerializeField] private int _topIndent = 0;

        public override bool CanSpawnInCell(Map map, RoomNode room, RoomData roomData)
        {
            return room.Position.y == map.Rows - _topIndent - 1;
        }
    }
}
