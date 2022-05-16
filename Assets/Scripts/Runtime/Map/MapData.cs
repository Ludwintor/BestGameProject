using System;
using UnityEngine;

namespace ProjectGame.DungeonMap
{
    [CreateAssetMenu(fileName = "New MapData", menuName = "Game/Map/MapData")]
    public class MapData : ScriptableObject
    {
        public int Rows => _rows;
        public int Columns => _columns;
        public int Density => _density;
        public int MaxStartNodeCount => _maxStartNodeCount;
        public int MaxAncestorDepth => _maxAncestorDepth;
        public RoomData[] Rooms => _rooms;

        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        [SerializeField] private int _density;
        [SerializeField] private int _maxStartNodeCount;
        [SerializeField] private int _maxAncestorDepth;
        [SerializeField] private RoomData[] _rooms;
        [SerializeField] private SpawnRule[] _spawnRules;

        public Map GenerateMap(RNG rng)
        {
            Map map = MapGenerator.Generate(_rows, _columns, _density, _maxStartNodeCount, _maxAncestorDepth, rng);
            RoomGenerator.Generate(map, _rooms, _spawnRules, rng);
            return map;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _rows = Mathf.Max(0, _rows);
            _columns = Mathf.Max(0, _columns);
            _density = Mathf.Max(0, _density);
            _maxStartNodeCount = Mathf.Max(0, _maxStartNodeCount);
            _maxAncestorDepth = Mathf.Max(0, _maxAncestorDepth);
        }
#endif
    }
}
