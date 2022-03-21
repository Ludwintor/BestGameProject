using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Cards
{
    public class Hand
    {
        public HandView View => _view;
        public bool IsFull => _cards.Count >= _maxCards;

        private List<Card> _cards;
        private HandView _view;
        private int _maxCards;

        public Hand(HandView handView, int maxCards = 10)
        {
            _view = handView;
            _maxCards = maxCards;
            _cards = new List<Card>(maxCards);
        }

        public void Add(Card card)
        {
            _cards.Add(card);
            _view.AddView(card.View);
        }

        public void Remove(Card card)
        {
            _cards.Remove(card);
            _view.RemoveView(card.View);
        }

        public Card RemoveLast()
        {
            Card card = _cards[_cards.Count - 1];
            _cards.RemoveAt(_cards.Count - 1);
            _view.RemoveView(card.View);
            return card;
        }
    }
}
