using ProjectGame.Cards;
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
        [SerializeField] private Button _drawButton;
        [SerializeField] private Button _removeButton;

        private Hand _hand;
        private Character _enemy;
        private System.Random _rng;

        private void Start()
        {
            _rng = new System.Random();
            _hand = new Hand(_handView);
            _drawButton.onClick.AddListener(AddCard);
            _removeButton.onClick.AddListener(RemoveCard);
        }

        private void AddCard()
        {
            CardView view = Instantiate(_prefab, null);
            CardData randomData = _data[_rng.Next(_data.Count)];
            Card card = new Card(randomData, view);
            _hand.Add(card);
        }

        private void RemoveCard()
        {
            Card last = _hand.RemoveLast();
            Destroy(last.View.gameObject);
        }
    }
}
