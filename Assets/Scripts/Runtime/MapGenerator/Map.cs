using System;
using System.Collections.Generic;

namespace ProjectGame
{
    public class Map
    {
        [Flags]
        public enum CellState
        {
            PathDownLeft = 1, //0001
            PathDown = 2, //0010
            PathDownRight = 4, //0100

            Activated = 8 //1000
        }

        private readonly struct Neighbour
        {
            public readonly CellState DirectionToNeighbour;
            public readonly Position Position;

            public Neighbour(CellState directionToNeighbour, Position position)
            {
                DirectionToNeighbour = directionToNeighbour;
                Position = position;
            }
        }

        private readonly CellState[,] _grid;
        private readonly int _width;
        private readonly int _height;
        private readonly int _expectedRoomCount;
        private readonly Random _rng;

        public Map(int width, int height, int expectedRoomCount, int seed)
        {
            _width = width;
            _height = height;
            _grid = new CellState[width, height];
            _expectedRoomCount = expectedRoomCount;
            _rng = new Random(seed);
        }

        public void GenerateMap(int rootPosition)
        {
            Position root = new Position(rootPosition, _height - 1);
            List<Position> mainBranch = GenerateBranchList(root);


            mainBranch.RemoveAt(mainBranch.Count - 1);
            for (int i = 0; i < (int)(mainBranch.Count * 0.2f); i++)
                mainBranch.RemoveAt(_rng.Next(0, mainBranch.Count - 1));
            int roomCount = mainBranch.Count;

            while (roomCount < _expectedRoomCount && mainBranch.Count > 0)
            {
                int nextBranchIndex = _rng.Next(0, mainBranch.Count - 1);
                Position nextBranch = mainBranch[nextBranchIndex];
                mainBranch.RemoveAt(nextBranchIndex);
                GenerateBranch(nextBranch);
                roomCount = GetRoomCount();
            }
        }
        
        public void GenerateMap()
        {
            int rootOffset = (int)(_width * 0.25f);
            GenerateMap(_rng.Next(rootOffset, _width - rootOffset));
        }

        public int GetRoomCount()
        {
            int count = 0;
            for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
                if (_grid[x, y].HasFlag(CellState.Activated))
                    count++;

            return count;
        }

        public CellState[,] Grid => _grid;
        public int Width => _width;
        public int Height => _height;

        private void GenerateBranch(Position startCell)
        {
            Position currentCell = startCell;
            _grid[currentCell.x, currentCell.y] |= CellState.Activated;

            while (currentCell.y > 0)
            {
                List<Neighbour> neighbours = GetAccessibleCells(currentCell);
                List<Neighbour> unactivatedNeighbours = GetUnactivatedNeighbours(neighbours);

                Neighbour chosenNeighbour;
                if (unactivatedNeighbours.Count > 0)
                    chosenNeighbour = unactivatedNeighbours[_rng.Next(0, unactivatedNeighbours.Count)];
                else
                    chosenNeighbour = neighbours[_rng.Next(0, neighbours.Count)];

                _grid[currentCell.x, currentCell.y] |= chosenNeighbour.DirectionToNeighbour;
                currentCell = chosenNeighbour.Position;
                _grid[currentCell.x, currentCell.y] |= CellState.Activated;
            }
        }

        private List<Position> GenerateBranchList(Position startCell)
        {
            List<Position> positions = new List<Position>();
            Position currentCell = startCell;
            _grid[currentCell.x, currentCell.y] |= CellState.Activated;

            while (currentCell.y > 0)
            {
                positions.Add(currentCell);
                List<Neighbour> neighbours = GetAccessibleCells(currentCell);
                List<Neighbour> unactivatedNeighbours = GetUnactivatedNeighbours(neighbours);

                Neighbour chosenNeighbour;
                if (unactivatedNeighbours.Count > 0)
                    chosenNeighbour = unactivatedNeighbours[_rng.Next(0, unactivatedNeighbours.Count)];
                else
                    chosenNeighbour = neighbours[_rng.Next(0, neighbours.Count)];

                _grid[currentCell.x, currentCell.y] |= chosenNeighbour.DirectionToNeighbour;
                positions.Add(currentCell);
                currentCell = chosenNeighbour.Position;
                _grid[currentCell.x, currentCell.y] |= CellState.Activated;
            }

            return positions;
        }

        private List<Neighbour> GetAccessibleCells(Position currentCell)
        {
            List<Neighbour> neighbours = new List<Neighbour>();
            if (currentCell.y - 1 >= 0)
            {
                neighbours.Add(new Neighbour(CellState.PathDown, currentCell + Position.Zero.Down()));
                if (currentCell.x - 1 > 0 &&
                    !_grid[currentCell.x - 1, currentCell.y].HasFlag(CellState.PathDownRight))
                    neighbours.Add(new Neighbour(CellState.PathDownLeft, currentCell + Position.Zero.Down().Left()));

                if (currentCell.x + 1 < _width &&
                    !_grid[currentCell.x + 1, currentCell.y].HasFlag(CellState.PathDownLeft))
                    neighbours.Add(new Neighbour(CellState.PathDownRight, currentCell + Position.Zero.Down().Right()));
            }

            return neighbours;
        }

        private List<Neighbour> GetUnactivatedNeighbours(List<Neighbour> neighbours)
        {
            List<Neighbour> unactivatedNeighbours = new List<Neighbour>();
            for (int i = neighbours.Count - 1; i >= 0; i--)
            {
                Position neighbourPos = neighbours[i].Position;
                if (!_grid[neighbourPos.x, neighbourPos.y].HasFlag(CellState.Activated))
                    unactivatedNeighbours.Add(neighbours[i]);
            }

            return unactivatedNeighbours;
        }

        public struct Position
        {
            public int x;
            public int y;

            public Position(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            #region Offsets

            public Position Down()
            {
                y -= 1;
                return this;
            }

            public Position Up()
            {
                y += 1;
                return this;
            }

            public Position Right()
            {
                x += 1;
                return this;
            }

            public Position Left()
            {
                x -= 1;
                return this;
            }

            #endregion

            public static Position Zero => new Position(0, 0);

            public static Position operator +(Position a, Position b)
            {
                a.x += b.x;
                a.y += b.y;
                return a;
            }
        }
    }
}