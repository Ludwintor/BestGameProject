using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace ProjectGame.DungeonMap
{
    public class RoomGenerator
    {
        private const float DEFAULT_SPECIAL_ENEMY_ROOM_PERCENTAGE = 0.1f;
        private const float DEFAULT_EVENT_ROOM_PERCENTAGE = 0.1f;
        private const float DEFAULT_SHOP_ROOM_PERCENTAGE = 0.1f;
        private const float DEFAULT_REWARD_ROOM_PERCENTAGE = 0.1f;
        private const float DEFAULT_REST_ROOM_PERCENTAGE = 0.1f;

        private readonly Dictionary<RoomType, int> roomCount = new Dictionary<RoomType, int>();
        private List<RoomType> _roomTypes = new List<RoomType>();
        private readonly Map _map;
        private readonly List<RoomNode> _roomNodes = new List<RoomNode>();
        private readonly int _lastRow;
        private readonly int _lastButOneRow;
        private readonly int _middleRow;
        private readonly Random _rng;

        public RoomGenerator(Map map, Random rng)
        {
            _map = map;

            _lastRow = _map.Rows - 1;
            _lastButOneRow = _map.Rows - 2;
            _middleRow = _map.Rows / 2;
            _rng = rng;

            foreach (RoomNode roomNode in map)
            {
                if (!roomNode.HasConnection) continue;
                if (roomNode.Position.y == _lastRow || roomNode.Position.y == _lastButOneRow ||
                    roomNode.Position.y == _middleRow || roomNode.Position.y == 0) continue;

                _roomNodes.Add(roomNode);
            }


            int roomNumber = _roomNodes.Count;
            roomCount.Add(RoomType.SpecialEnemy, Mathf.FloorToInt(roomNumber * DEFAULT_SPECIAL_ENEMY_ROOM_PERCENTAGE));
            roomCount.Add(RoomType.Event, Mathf.FloorToInt(roomNumber * DEFAULT_EVENT_ROOM_PERCENTAGE));
            roomCount.Add(RoomType.Shop, Mathf.FloorToInt(roomNumber * DEFAULT_SHOP_ROOM_PERCENTAGE));
            roomCount.Add(RoomType.Reward, Mathf.FloorToInt(roomNumber * DEFAULT_REWARD_ROOM_PERCENTAGE));
            roomCount.Add(RoomType.Rest, Mathf.FloorToInt(roomNumber * DEFAULT_REST_ROOM_PERCENTAGE));
        }

        public void SetRoomCount(RoomType roomType, int count)
        {
            if (roomCount.ContainsKey(roomType))
            {
                roomCount[roomType] = count;
            }
            else
            {
                roomCount.Add(roomType, count);
            }
        }

        public Map Generate()
        {
            _roomTypes = new List<RoomType>(roomCount.Keys);
            foreach (var roomNode in _map.GetRow(_lastRow))
            {
                if (!roomNode.HasConnection) continue;
                roomNode.Type = RoomType.Boss;
                break;
            }

            foreach (var roomNode in _map.GetRow(_lastButOneRow)) roomNode.Type = RoomType.Rest;

            foreach (var roomNode in _map.GetRow(_middleRow))
                if (roomNode.HasConnection)
                    roomNode.Type = RoomType.Reward;

            while (_roomNodes.Count > 0 && _roomTypes.Count > 0)
            {
                RoomNode rngNode = _roomNodes[_rng.Next(_roomNodes.Count)];
                rngNode.Type = GetRoomToPlace();
                _roomNodes.Remove(rngNode);
            }

            return _map;
        }

        private RoomType GetRoomToPlace()
        {
            RoomType roomType = _roomTypes[_rng.Next(_roomTypes.Count)];
            roomCount[roomType] -= 1;
            if (roomCount[roomType] <= 0)
            {
                _roomTypes.Remove(roomType);
                roomCount.Remove(roomType);
            }

            return roomType;
        }
        
    }
}