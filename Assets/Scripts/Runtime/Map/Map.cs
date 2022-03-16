using System.Collections;
using System.Collections.Generic;

namespace ProjectGame.DungeonMap
{
    public class Map : IEnumerable<RoomNode>
    {
        public int Rows { get; }
        public int Columns { get; }
        private readonly RoomNode[][] _map;

        public Map(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _map = new RoomNode[rows][];
            for (int y = 0; y < rows; y++)
            {
                _map[y] = new RoomNode[columns];
                for (int x = 0; x < columns; x++)
                    _map[y][x] = new RoomNode(x, y);
            }
        }

        public RoomNode GetNode(int x, int y) => _map[y][x];

        public RoomNode[] GetRow(int row) => _map[row];

        public IEnumerator<RoomNode> GetEnumerator()
        {
            for (int y = 0; y < Rows; y++)
                for (int x = 0; x < Columns; x++)
                    yield return GetNode(x, y);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
