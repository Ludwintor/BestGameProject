using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectGame.Powers;
using UnityEngine;

namespace ProjectGame.Characters
{
    public class CharacterManager : MonoBehaviour, ISystem
    {
        [Header("Enemy")]
        [SerializeField] private EnemyView _enemyPrefab;
        [SerializeField] private SpawnPoint[] _enemySpawnPoints;

        [SerializeField] private PowerData _strengthData; // TODO: REMOVE!!!

        private List<EnemyView> _enemyViews = new List<EnemyView>();

        private void Awake()
        {
            Game.RegisterSystem(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Game.Dungeon.Player.PowerGroup.Add(_strengthData, 2);
            }
        }

        public Enemy SpawnEnemy(EnemyData data)
        {
            Enemy enemy = new Enemy(data);
            SpawnPoint freePoint = GetFreeSpawnPoint();
            freePoint.Occupier = enemy;
            EnemyView view = Instantiate(_enemyPrefab, freePoint.transform.position, Quaternion.identity);
            view.Init(enemy);
            _enemyViews.Add(view);
            return enemy;
        }

        public void DespawnEnemy(Enemy enemy)
        {
            EnemyView view = _enemyViews.FirstOrDefault(v => v.Enemy == enemy);
            SpawnPoint point = _enemySpawnPoints.FirstOrDefault(p => p.Occupier == enemy);
            point.Occupier = null;
            _enemyViews.Remove(view);
            Destroy(view.gameObject);
        }

        private SpawnPoint GetFreeSpawnPoint()
        {
            return _enemySpawnPoints.FirstOrDefault(point => !point.IsOccupied);
        }
    }
}
