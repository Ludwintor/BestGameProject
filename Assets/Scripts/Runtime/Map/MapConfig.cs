using ProjectGame.DungeonMap;
using UnityEngine;

namespace ProjectGame
{
    [CreateAssetMenu(fileName = "New MapData", menuName = "Game/Map/MapConfig")]
    public partial class MapConfig : ScriptableObject
    {
        [Header("Dimensions")] [SerializeField]
        private int _rows = 10;
        [SerializeField] private int _columns = 5;
        
        [Header("Settings")] [SerializeField] private int _density = 10;
        [SerializeField] private int _maxStartNodeCount = 3;
        [SerializeField] private int _maxAncestorDepth = 4;
        [SerializeField] private RoomData[] _rooms;
        [SerializeField] private SpawnRule[] _spawnRules;

        public int Rows => _rows;
        public int Columns => _columns;
        public int Density => _density;
        public int maxStartNodeCount => _maxStartNodeCount;
        public int MaxAncestorDepth => _maxAncestorDepth;
        public RoomData[] Rooms => _rooms;

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