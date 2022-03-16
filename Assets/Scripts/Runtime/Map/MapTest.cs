using System.Diagnostics;
using UnityEngine;
using ProjectGame.DungeonMap;

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
        private Map _map;

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            Stopwatch timer = Stopwatch.StartNew();
            _map = MapGenerator.GenerateMap(_height, _width, _density, _maxStart, _maxAncestorDepth, new System.Random(_seed));
            timer.Stop();
            UnityEngine.Debug.Log($"Map generation finished in {timer.ElapsedTicks / 100f}ns");
        }

        public void GenerateSeed()
        {
            _seed = Random.Range(0, 100000);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_map == null) 
                return;

            Gizmos.color = Color.gray;
            for (int x = 0; x <= _width; x++)
            {
                Gizmos.DrawLine(new Vector3(x, 0), new Vector3(x, _height));
            }
            for (int y = 0; y <= _height; y++)
            {
                Gizmos.DrawLine(new Vector3(0, y), new Vector3(_width, y));
            }
            Gizmos.color = Color.red;
            foreach (RoomNode node in _map)
            {
                foreach (RoomNode outgoing in node.ChildrenNodes)
                {
                    Gizmos.DrawLine(new Vector3(node.Position.x + 0.5f, node.Position.y + 0.5f), new Vector3(outgoing.Position.x + 0.5f, outgoing.Position.y + 0.5f));
                }
            }
        }
    }
}