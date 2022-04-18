using System.Collections;
using System.Collections.Generic;
using ProjectGame.DungeonMap;
using UnityEngine;

namespace ProjectGame
{
    [CreateAssetMenu(menuName = "Game/Rooms/SpawnRules/BottomIndent")]
    public class BottomIndentBaseSpawnRule : BaseSpawnRule
    {
        public int bottomIndent => _bottomIndent;
        [SerializeField] private int _bottomIndent = 0;
        private BaseSpawnRule _baseSpawnRuleImplementation;

        public override bool CanSpawnInCell(Map map, RoomNode room, RoomData data)
        {
            return room.Position.y == bottomIndent;
        }
    }
}
