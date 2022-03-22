using System;
using System.Diagnostics;
using UnityEngine;
using ProjectGame.DungeonMap;
using Random = UnityEngine.Random;

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
        [SerializeField] private MeshFilter _meshFilter;

        private Map _map;
        private ShapeMesh _shapeMesh;

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            Stopwatch timer = Stopwatch.StartNew();
            System.Random rng = new System.Random(_seed);
            _map = MapGenerator.GenerateMap(_height, _width, _density, _maxStart, _maxAncestorDepth, rng);
            RoomGenerator generator = new RoomGenerator(_map, rng);
            generator.SetRoomCount(RoomType.Rest, 4);
            generator.Generate();
            
            timer.Stop();
            UnityEngine.Debug.Log($"Map generation finished in {timer.ElapsedTicks / 100f}ns");

            _shapeMesh = new ShapeMesh();
            _meshFilter.mesh = _shapeMesh.GetMesh();
            foreach (RoomNode node in _map)
            {
                if (node.HasConnection)
                {
                    Vector3 position = new Vector3(node.Position.x + 0.5f, node.Position.y + 0.5f);
                    _shapeMesh.DrawShape(position, Vector2.one, (int)node.Type);
                }
            }
            _shapeMesh.UpdateMesh();
        }

        public void GenerateSeed()
        {
            _seed = Random.Range(0, Int32.MaxValue);
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
            // Gizmos.DrawGUITexture(new Rect(Vector2.zero, Vector2.one), _sprite.texture);
        }
    }
}