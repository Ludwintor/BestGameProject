using System;
using System.Collections.Generic;
using ProjectGame.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class CardPickMenu : MonoBehaviour
    {
        [SerializeField] private Transform _cardParent;
        [SerializeField] private Button _skipButton;
        private readonly List<CardView> _cardViews = new List<CardView>();
        private Action<Card, bool> _picked;

        private void Awake()
        {
            _skipButton.onClick.AddListener(Skip);
        }

        public void PickACard(IEnumerable<Card> cards, Action<Card, bool> picked)
        {
            _picked = picked;
            foreach (Card card in cards)   
            {
                CardView cardView = Game.CardsPool.Get();
                cardView.Init(card);
                cardView.gameObject.SetActive(true);
                cardView.transform.SetParent(_cardParent, false);
                cardView.SetDragEnabled(false);
                _cardViews.Add(cardView);
                cardView.PointerDown += OnClick;
            }
        }

        private void Skip()
        {
            foreach (CardView cardView in _cardViews)
            {
                cardView.PointerDown -= OnClick;
                Game.CardsPool.Release(cardView);
            }
            _cardViews.Clear();
            _picked(null, false);
        }
        
        private void OnClick(Card card)
        {
            foreach (CardView cardView in _cardViews)
            {
                cardView.PointerDown -= OnClick;
                Game.CardsPool.Release(cardView);
            }
            _cardViews.Clear();
            _picked(card, true);
        }
    }
}
