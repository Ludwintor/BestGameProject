using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Cards
{
    public class Hand
    {
        private List<Card> _cards = new List<Card>(10);

        private HandView _view;

        public Hand(HandView handView)
        {
            _view = handView;
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
