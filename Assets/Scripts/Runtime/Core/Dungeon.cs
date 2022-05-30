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
        public List<Enemy> Enemies => _enemies;

        private Player _player;
        private RoomNode _currentRoom;
        private List<Enemy> _enemies;
        private Map _map;
        private RNG _mapRandom;
        private RNG _enemyRandom;

        public Dungeon(Player player, MapData data)
        {
            _player = player;
            _mapRandom = new RNG();
            _enemyRandom = new RNG();
            _enemies = new List<Enemy>();
            _map = data.GenerateMap(_mapRandom);
        }

        public void SelectNextRoom()
        {
            MapWindow window = Game.GetSystem<UIManager>().MapWindow;
            window.Show(_map);
            window.AllowSelectNextRoom(_currentRoom, RoomSelected);
        }

        private void CombatStart()
        {

        }

        private void CombatVictory()
        {
            // TODO: Give rewards to player. But now just show map
            Game.GetSystem<TurnManager>().EndCombat();
            Game.GetSystem<UIManager>().HideHand();
            SelectNextRoom();
        }

        private void RoomSelected(RoomNode room)
        {
            CharacterManager characterManager = Game.GetSystem<CharacterManager>();
            UIManager uiManager = Game.GetSystem<UIManager>();
            _currentRoom = room;
            _currentRoom.MarkAsVisited();
            _enemies.Clear();
            // TODO: Instead of this check, add enum with each room type
            if (_currentRoom.RoomType != RoomType.CommonEnemy)
                return;
            foreach (EnemyData enemyData in _currentRoom.GetRandomEnemies(_enemyRandom))
            {
                Enemy enemy = characterManager.SpawnEnemy(enemyData);
                enemy.Dead += OnEnemyDead;
                _enemies.Add(enemy);
            }
            uiManager.ShowHand();
            uiManager.MapWindow.Hide();
            Game.GetSystem<TurnManager>().StartCombat();
        }

        private void OnEnemyDead(Character character)
        {
            CharacterManager characterManager = Game.GetSystem<CharacterManager>();
            Enemy enemy = (Enemy)character;
            characterManager.DespawnEnemy(enemy);
            enemy.Dead -= OnEnemyDead;
            foreach (Enemy roomEnemy in _enemies)
                if (roomEnemy.IsAlive)
                    return;
            CombatVictory();
        }
    }
}
