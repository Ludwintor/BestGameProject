using System;
using System.Collections.Generic;
using ProjectGame.DungeonMap;
using UnityEngine;

namespace ProjectGame
{
    [Serializable]
    public struct SpawnRule
    {
        [SerializeField] private RangeInt[] _affectedRows;
        [SerializeField] private FlagRoomType _flagRoomType;
        [Range(0, 100)] [SerializeField] private int _spawnChance;
        [SerializeField] private bool _canBeOverriden;
        [SerializeField] private bool _canSpawnNextToItself;

        public bool CanBeOverriden => _canBeOverriden;

        public bool CanSpawnInRow(Vector2Int position)
        {
            foreach (RangeInt affectedRow in _affectedRows)
            {
                if (affectedRow.min <= position.y && position.y <= affectedRow.max) return true;
            }

            return false;
        }

        public bool CanSpawnInCell(RoomNode room, RoomType roomType)
        {
            if (!CanSpawnInRow(room.Position)) return false;

            if (_canSpawnNextToItself) return true;

            foreach (RoomNode parentRoom in room.ParentNodes)
            {
                if (parentRoom.roomType == roomType) return false;
            }

            foreach (RoomNode childRoom in room.ChildrenNodes)
            {
                if (childRoom.roomType == roomType) return false;
            }

            return true;
        }

        public bool SpawnInCell(RoomNode room, RoomType data, RNG rng)
        {
            return CanSpawnInCell(room, data) && rng.NextInt(101) <= _spawnChance;
        }

        [Flags]
        private enum FlagRoomType
        {
            CommonEnemy = 1,
            SpecialEnemy = 2,
            Shop = 4,
            Reward = 8,
            Rest = 16,
            Event = 32,
            Boss = 64
        }

        public IEnumerable<RoomType> GetRoomTypes()
        {
            foreach (FlagRoomType type in Enum.GetValues(typeof(FlagRoomType)))
            {
                if (!_flagRoomType.HasFlag(type)) continue;

                yield return (RoomType) (int) Mathf.Log((int) type, 2);
            }
        }
    }
}