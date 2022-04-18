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
    public class TestCard : MonoBehaviour
    {
        [Header("Cards")]
        [SerializeField] private List<CardData> _data;
        [SerializeField] private HandView _handView;
        [SerializeField] private DeckView _drawView;
        [SerializeField] private DeckView _discardView;
        [SerializeField] private TargetingSystem _targetingSystem;
        [Header("Enemy")]
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private EnemyView _enemyPrefab;
        [SerializeField] private Transform _enemyStart;
        [Header("Some UI")]
        [SerializeField] private Button _drawButton;
        [SerializeField] private float _drawDelay;

        private Player _player;
        private Enemy _enemy;
        private RNG _rng;

        private void Start()
        {
            Game.StartGame();
            _rng = new RNG();
            _player = new Player(_targetingSystem);
            _player.SetupViews(_handView, null, _drawView, _discardView);
            _enemy = new Enemy(_enemyData);
            EnemyView enemyView = Instantiate(_enemyPrefab, _enemyStart.position, Quaternion.identity);
            enemyView.Init(_enemy);
            Game.Dungeon.Enemies = new List<Enemy> { _enemy };
            _drawButton.onClick.AddListener(DrawCard);
            Game.Dungeon.InitPlayer(_player);
            for (int i = 0; i < 10; i++)
                AddCard();
            Game.GetSystem<TurnManager>().StartTurn();
        }

        private void AddCard()
        {
            CardData randomData = _data[_rng.NextInt(_data.Count)];
            Card card = new Card(randomData);
            _player.DrawDeck.Add(card);
        }

        private void DrawCard()
        {
            ActionManager actionManager = Game.GetSystem<ActionManager>();
            actionManager.AddToBottom(new DrawCardAction(_player, 0.2f, 1));
        }
    }
}
