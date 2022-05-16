using ProjectGame.Cards;
using ProjectGame.Characters;
using ProjectGame.DungeonMap;
using ProjectGame.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class Game : MonoBehaviour
    {
        private const string MAP_DATA_PATH = "MapData";

        public static Dungeon Dungeon => _dungeon;
        public static ObjectPool<CardView> CardsPool => _cardsPool;

        private static Dictionary<Type, ISystem> _systems = new Dictionary<Type, ISystem>();
        private static Dungeon _dungeon;
        private static ObjectPool<CardView> _cardsPool;

        [SerializeField] private CardView _cardPrefab;

        private void Start()
        {
            _cardsPool = new ObjectPool<CardView>(() => Instantiate(_cardPrefab, null),
                                                  view => view.Deactivate(),
                                                  view => Destroy(view.gameObject));
        }

        /// <summary>
        /// Add system in dictionary that can be accessable globally
        /// </summary>
        public static void RegisterSystem(ISystem system)
        {
            Type systemType = system.GetType();
            if (_systems.ContainsKey(systemType))
            {
                Debug.LogError($"[{nameof(Game)}.{nameof(RegisterSystem)}] {systemType.Name} already in system register");
                return;
            }
            _systems.Add(systemType, system);
        }

        /// <summary>
        /// Access certain system
        /// </summary>
        public static T GetSystem<T>() where T : ISystem
        {
            return (T)_systems[typeof(T)];
        }

        public static void StartGame(PlayerData playerData, MapData mapData)
        {
            _dungeon = new Dungeon(new Player(playerData), mapData);
            SceneLoader.LoadScene(SceneIndexes.Game);
        }
    }
}
