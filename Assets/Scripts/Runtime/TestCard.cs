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
        [Header("Player")]
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private PlayerView _playerView;
        [Header("Enemy")]
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private EnemyView _enemyPrefab;
        [SerializeField] private Transform _enemyStart;
        [Header("Some UI")]
        [SerializeField] private Button _drawButton;
        [SerializeField] private float _drawDelay;

        private Player _player;
        private Enemy _enemy;

        private void Start()
        {
            Game.StartGame();
            // TODO: Transfer most part of this code to Game.StartGame()
            PlayerUI playerUI = Game.GetSystem<UIManager>().PlayerUI;
            _player = new Player(_playerData);
            playerUI.Init(_player);
            _playerView.Init(_player);
            _enemy = new Enemy(_enemyData);
            EnemyView enemyView = Instantiate(_enemyPrefab, _enemyStart.position, Quaternion.identity);
            enemyView.Init(_enemy);
            Game.Dungeon.Enemies = new List<Enemy> { _enemy };
            _drawButton.onClick.AddListener(DrawCard);
            Game.Dungeon.InitPlayer(_player);
            for (int i = 0; i < _playerData.StartingCards.Length; i++)
                AddCard(_playerData.StartingCards[i]);
            _player.DrawDeck.Shuffle();
            Game.GetSystem<TurnManager>().StartTurn();
        }

        private void AddCard(CardData cardData)
        {
            Card card = new Card(cardData);
            _player.DrawDeck.Add(card);
        }

        private void DrawCard()
        {
            ActionManager actionManager = Game.GetSystem<ActionManager>();
            actionManager.AddToBottom(new DrawCardAction(_player, 0.2f, 1));
        }
    }
}
