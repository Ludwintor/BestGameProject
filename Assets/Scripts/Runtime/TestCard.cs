using ProjectGame.Cards;
using ProjectGame.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class TestCard : MonoBehaviour
    {
        [SerializeField] private List<CardData> _data;
        [SerializeField] private CardView _prefab;
        [SerializeField] private HandView _handView;
        [SerializeField] private TargetingSystem _targetingSystem;
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private Button _drawButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Button _drawThreeButton;
        [SerializeField] private float _drawDelay;

        private Player _player;
        private Enemy _enemy;
        private System.Random _rng;

        private void Start()
        {
            _rng = new System.Random();
            Hand hand = new Hand(_handView);
            _player = new Player(hand, _targetingSystem);
            _enemy = new Enemy(_enemyView);
            _drawButton.onClick.AddListener(AddCard);
            _removeButton.onClick.AddListener(RemoveCard);
            _drawThreeButton.onClick.AddListener(DrawThreeCards);
        }

        private void AddCard()
        {
            CardView view = Instantiate(_prefab, null);
            CardData randomData = _data[_rng.Next(_data.Count)];
            Card card = new Card(randomData, view);
            _player.Hand.Add(card);
        }

        private void RemoveCard()
        {
            Card last = _player.Hand.RemoveLast();
            Destroy(last.View.gameObject);
        }

        private void DrawThreeCards()
        {
            StartCoroutine(CardDrawing(3));
        }

        private IEnumerator CardDrawing(int count)
        {
            var delay = new WaitForSeconds(_drawDelay);
            for (int i = 0; i < count; i++)
            {
                AddCard();
                yield return delay;
            }
        }
    }
}
