using System.Collections.Generic;
using ProjectGame.Characters;
using ProjectGame.DungeonMap;

namespace ProjectGame
{
    public class Dungeon
    {
        public Player Player => _player;
        public RoomNode CurrentRoom => _currentRoom;
        public RNG EnemyRandom => _enemyRandom;
        // TODO: TEMP PROPERTY
        public List<Enemy> Enemies { get; set; }

        private Player _player;
        private RoomNode _currentRoom;
        private Map _map;
        private RNG _mapRandom;
        private RNG _enemyRandom;

        public Dungeon(Player player)
        {
            _player = player;
        }

        public void GenerateDungeon(MapData data)
        {
            _mapRandom = new RNG();
            _enemyRandom = new RNG();
            _map = data.GenerateMap(_mapRandom);
        }

        public void SelectNextRoom()
        {
            UIManager uiManager = Game.GetSystem<UIManager>();
        }
    }
}
