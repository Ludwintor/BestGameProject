using ProjectGame.Actions;
using ProjectGame.Cards;
using ProjectGame.Characters;
using ProjectGame.DungeonMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class DungeonBooter : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;
        [Header("Enemy")]
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private EnemyView _enemyPrefab;
        [SerializeField] private Transform _enemyStart;

        private Enemy _enemy;

        private void Start()
        {
            Player player = Game.Dungeon.Player;
            _playerView.Init(player);
            _enemy = new Enemy(_enemyData);
            EnemyView enemyView = Instantiate(_enemyPrefab, _enemyStart.position, Quaternion.identity);
            enemyView.Init(_enemy);
            Game.Dungeon.Enemies = new List<Enemy> { _enemy };
        }
    }
}
