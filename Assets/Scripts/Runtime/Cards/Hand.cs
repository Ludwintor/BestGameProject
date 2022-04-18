using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProjectGame.Cards
{
    public class Hand
    {
        public HandView View { get; set; }
        public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();
        public bool IsFull => _cards.Count >= _maxCards;
        public int Count => _cards.Count;

        private List<Card> _cards;
        private int _maxCards;

        public Hand()
        {
            _cards = new List<Card>();
        }

        public void Add(Card card)
        {
            _cards.Add(card);
            View.AttachView(card);
        }

        public void Remove(Card card)
        {
            _cards.Remove(card);
            View.DeattachView(card);
        }

        public Card RemoveLast()
        {
            Card card = _cards[_cards.Count - 1];
            Remove(card);
            return card;
        }

        public Card this[int i] => _cards[i];
    }
}
