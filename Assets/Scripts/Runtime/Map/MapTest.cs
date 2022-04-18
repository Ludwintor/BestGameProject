using System.Collections.Generic;
using System.Diagnostics;
using ProjectGame.DungeonMap;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace ProjectGame
{
    public class MapTest : MonoBehaviour
    {
        [SerializeField] private int _width = 5;
        [SerializeField] private int _height = 10;
        [SerializeField] private int _density = 10;
        [SerializeField] private int _maxStart = 3;
        [SerializeField] private int _maxAncestorDepth = 4;
        [SerializeField] private int _seed;
        [SerializeField] private List<RoomData> _rooms;
        [SerializeField] private Material _spriteMaterial;
        [SerializeField] private MapUI _mapUI;

        private Map _map;

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            Stopwatch timer = Stopwatch.StartNew();

            RNG rng = new RNG(_seed);
            _map = MapGenerator.Generate(_height, _width, _density, _maxStart, _maxAncestorDepth, rng);
            RoomGenerator.Generate(_map, _rooms, rng);

            timer.Stop();
            Debug.Log($"Map generation finished in {timer.ElapsedTicks / 100f}ns");
            _mapUI.GenerateUI(_map, rng);
        }

        public void GenerateSeed()
        {
            _seed = UnityEngine.Random.Range(0, int.MaxValue);
        }
    }
}