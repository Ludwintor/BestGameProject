using System.Collections.Generic;
using ProjectGame.Characters;
using ProjectGame.DungeonMap;
using ProjectGame.Windows;

namespace ProjectGame
{
    public class Dungeon
    {
        public Player Player => _player;
        public RoomNode CurrentRoom => _currentRoom;
        public Map Map => _map;
        public RNG EnemyRandom => _enemyRandom;
        // TODO: TEMP PROPERTY
        public List<Enemy> Enemies { get; set; }

        private Player _player;
        private RoomNode _currentRoom;
        private Map _map;
        private RNG _mapRandom;
        private RNG _enemyRandom;

        public Dungeon(Player player, MapData data)
        {
            _player = player;
            _mapRandom = new RNG();
            _enemyRandom = new RNG();
            _map = data.GenerateMap(_mapRandom);
        }

        public void SelectNextRoom()
        {
            MapWindow window = Game.GetSystem<UIManager>().MapWindow;
            window.Show(_map);
            window.AllowSelectNextRoom(_currentRoom, RoomSelected);
        }

        private void RoomSelected(RoomNode room)
        {
            _currentRoom = room;
            _currentRoom.MarkAsVisited();
        }
    }
}
