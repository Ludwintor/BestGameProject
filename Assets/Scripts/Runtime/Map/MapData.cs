using UnityEngine;

namespace ProjectGame.DungeonMap
{
    [CreateAssetMenu(fileName = "New MapData", menuName = "Game/Map/MapData")]
    public class MapData : ScriptableObject
    {
        public int Rows => _rows;
        public int Columns => _columns;
        public int Density => _density;
        public int MaxStart => _maxStart;
        public int MaxAncestorDepth => _maxAncestorDepth;

        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        [SerializeField] private int _density;
        [SerializeField] private int _maxStart;
        [SerializeField] private int _maxAncestorDepth;

        public Map GenerateMap(RNG rng)
        {
            return MapGenerator.Generate(_rows, _columns, _density, _maxStart, _maxAncestorDepth, rng);
        }
    }
}
