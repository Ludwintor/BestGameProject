using ProjectGame.DungeonMap;
using UnityEngine;

namespace ProjectGame
{
    [CreateAssetMenu(menuName = "Game/Rooms/SpawnRules/Random")]
    public class RandomSpawnRule : BaseSpawnRule
    {
        [SerializeField] private bool _canSpawnNextToItself;
        
        public override bool CanSpawnInCell(Map map, RoomNode room, RoomData data)
        {
            if (_canSpawnNextToItself) return true;

            foreach (RoomNode parentRoom in room.ParentNodes)
            {
                if (parentRoom.Data == data) return false;
            }

            foreach (RoomNode childRoom in room.ChildrenNodes)
            {
                if (childRoom.Data == data) return false;
            }

            return true;
        }
    }
}