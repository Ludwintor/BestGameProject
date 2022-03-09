using UnityEngine;

namespace ProjectGame
{
    public class MapTest : MonoBehaviour
    {
        [SerializeField] private int _width = 5;
        [SerializeField] private int _height = 10;
        [SerializeField] private int _expectedRoomCount = 25;
        [SerializeField] private int _seed;
        private Map _map;

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            _map = new Map(_width, _height, _expectedRoomCount, _seed);
            _map.GenerateMap(_width / 2);
            Debug.Log($"Expected room count: {_expectedRoomCount} Actual: {_map.GetRoomCount()}");
        }

        public void GenerateSeed()
        {
            _seed = Random.Range(0, 10000);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_map == null) return;
            Map.CellState[,] grid = _map.Grid;
            Gizmos.color = Color.gray;
            for (int x = 0; x <= _map.Width; x++)
            {
                Gizmos.DrawLine(new Vector3(x, 0f), new Vector3(x, _map.Height));
            }
            
            for (int y = 0; y <= _map.Height; y++)
            {
                Gizmos.DrawLine(new Vector3(0f, y), new Vector3(_map.Width, y));
            }

            
            for (int x = 0; x < _map.Width; x++)
            for (int y = _map.Height - 1; y >= 0; y--)
            {
                if (!grid[x, y].HasFlag(Map.CellState.Activated)) continue;
                Gizmos.color = Color.red;
                if (grid[x, y].HasFlag(Map.CellState.PathDown))
                {
                    Gizmos.DrawLine(new Vector3(0.5f + x, 0.5f + y), new Vector3(0.5f + x, 0.5f + y - 1f));
                }
                
                if (grid[x, y].HasFlag(Map.CellState.PathDownLeft))
                {
                    Gizmos.DrawLine(new Vector3(0.5f + x, 0.5f + y), new Vector3(0.5f + x - 1f, 0.5f + y - 1f));
                }
                
                if (grid[x, y].HasFlag(Map.CellState.PathDownRight))
                {
                    Gizmos.DrawLine(new Vector3(0.5f + x, 0.5f + y), new Vector3(0.5f + x + 1f, 0.5f + y - 1f));
                }
                
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(new Vector3(0.5f + x, 0.5f + y), 0.25f);
            }

            // List<Map.Path> paths = _map.GetPaths();
            //
            // foreach (Map.Path path in paths)
            // {
            //     Vector3 startPos = new Vector3(path.Start.x, path.Start.y);
            //     Vector3 endPos = new Vector3(path.End.x, path.End.y);
            //     Gizmos.DrawLine(startPos, endPos);
            // }
            //
            // List<Map.Node> nodes = new List<Map.Node>();
            // _map.GetNodes(nodes);
            // foreach (var node in nodes)
            // {
            //     Gizmos.DrawSphere(new Vector3(node.Position.x, node.Position.y), 0.25f);
            // }
        }
    }
}