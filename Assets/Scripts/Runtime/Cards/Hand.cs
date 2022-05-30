using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProjectGame.Cards
{
    public class Hand
    {
        public event System.Action<Card> CardDrawn;
        public event System.Action<Card> CardDiscarded;
        public event System.Action HandCleared;
        public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();
        public bool IsFull => _cards.Count >= _maxCards;
        public int Count => _cards.Count;

        private List<Card> _cards;
        private Deck _drawDeck;
        private Deck _discardDeck;
        private int _maxCards;

        public Hand(Deck drawDeck, Deck discardDeck)
        {
            _drawDeck = drawDeck;
            _discardDeck = discardDeck;
            _cards = new List<Card>();
        }

        public void Draw()
        {
            Card card = _drawDeck.TakeFromTop();
            _cards.Add(card);
            CardDrawn?.Invoke(card);
        }

        public void Discard(Card card)
        {
            _cards.Remove(card);
            _discardDeck.Add(card);
            CardDiscarded?.Invoke(card);
        }

        public void Clear()
        {
            _cards.Clear();
            HandCleared?.Invoke();
        }

        public Card DiscardLast()
        {
            Card card = _cards[^1];
            Discard(card);
            return card;
        }

        public Card this[int i] => _cards[i];
    }
}
