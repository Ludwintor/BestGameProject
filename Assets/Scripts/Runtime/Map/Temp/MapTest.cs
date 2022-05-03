using System.Diagnostics;
using ProjectGame.DungeonMap;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ProjectGame
{
    public class MapTest : MonoBehaviour
    {
        [SerializeField] private int _seed;
        [SerializeField] private MapUI _mapUI;
        [SerializeField] private MapConfig _mapConfig;


        private Map _map;

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            Stopwatch timer = Stopwatch.StartNew();
            RNG rng = new RNG(_seed);
            _map = _mapConfig.GenerateMap(rng);
            timer.Stop();
            
            Debug.Log($"Map generation finished in {timer.ElapsedTicks / 100f}ns");
            // _mapUI.GenerateUI(_map, rng);
        }

        public void GenerateSeed()
        {
            _seed = Random.Range(0, int.MaxValue);
        }
    }
}