using System.Collections.Generic;
using ProjectGame.Characters;
using ProjectGame.DungeonMap;
using ProjectGame.Windows;
using UnityEngine;

namespace ProjectGame
{
    public class Dungeon
    {
        public Sprite CardSprite { get; set; } // TODO: At now, used only for CardReward sprite, rework rewards completely and delete this
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
        private RNG _rewardsRandom;

        public Dungeon(Player player, MapData data)
        {
            _player = player;
            _mapRandom = new RNG();
            _enemyRandom = new RNG();
            _rewardsRandom = new RNG();
            _enemies = new List<Enemy>();
            _player.Dead += OnPlayerDead;
            _map = data.GenerateMap(_mapRandom);
        }

        public void SelectNextRoom()
        {
            MapWindow window = Game.GetSystem<UIManager>().MapWindow;
            window.Show(_map);
            window.AllowSelectNextRoom(_currentRoom, OnRoomSelected);
        }

        private void CombatStart()
        {

        }

        private void CombatVictory()
        {
            Game.GetSystem<TurnManager>().EndCombat();
            Game.GetSystem<UIManager>().HideHand();
            if (_currentRoom.RoomType == RoomType.Boss)
            {
                SceneLoader.LoadScene(SceneIndexes.MainMenu);
                return;
            }
            RewardManager rewardManager = Game.GetSystem<RewardManager>();
            rewardManager.AddReward(new CardReward(CardSprite, Game.CardDatabase.TakeRandom(_rewardsRandom, 3), 1));
            if (_currentRoom.RoomType == RoomType.SpecialEnemy)
                rewardManager.AddReward(new CardReward(CardSprite, Game.CardDatabase.TakeRandom(_rewardsRandom, 3), 1));
            rewardManager.AllRewardsGained += OnRewardsGained;
            rewardManager.GainAllRewards();
        }

        private void OnRewardsGained(RewardManager rewardManager)
        {
            rewardManager.AllRewardsGained -= OnRewardsGained;
            SelectNextRoom();
        }

        private void OnRoomSelected(RoomNode room)
        {
            CharacterManager characterManager = Game.GetSystem<CharacterManager>();
            UIManager uiManager = Game.GetSystem<UIManager>();
            _currentRoom = room;
            _enemies.Clear();
            // TODO: Let rooms handle it on their own instead of switch
            switch (room.RoomType)
            {
                case RoomType.CommonEnemy:
                case RoomType.SpecialEnemy:
                case RoomType.Boss:
                    foreach (EnemyData enemyData in _currentRoom.GetRandomEnemies(_enemyRandom))
                    {
                        Enemy enemy = characterManager.SpawnEnemy(enemyData);
                        enemy.Dead += OnEnemyDead;
                        _enemies.Add(enemy);
                    }
                    uiManager.ShowHand();
                    uiManager.MapWindow.Hide();
                    Game.GetSystem<TurnManager>().StartCombat();
                    break;
                case RoomType.Rest:
                    int heal = Mathf.RoundToInt(_player.MaxHealth / 3f);
                    _player.Heal(heal);
                    SelectNextRoom();
                    break;
                case RoomType.Shop:
                    break;
                case RoomType.Reward:
                    break;
                case RoomType.Event:
                    break;
                default:
                    Debug.Assert(false, $"Strange room type went in");
                    break;
            }
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

        private void OnPlayerDead(Character character)
        {
            Player player = (Player)character;
            player.Dead -= OnPlayerDead;
            SceneLoader.LoadScene(SceneIndexes.MainMenu);
        }
    }
}
