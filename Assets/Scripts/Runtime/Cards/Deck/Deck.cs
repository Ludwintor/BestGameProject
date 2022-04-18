using ProjectGame.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProjectGame.Cards
{
    public class Deck
    {
        public DeckView View { get; set; }
        public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();
        public int Count => _cards.Count;

        private List<Card> _cards;
        private RNG _shuffleRNG;

        public Deck()
        {
            _cards = new List<Card>();
            _shuffleRNG = new RNG();
        }

        public void Add(Card card)
        {
            _cards.Add(card);
            View.UpdateCount(_cards.Count);
        }

        public void Remove(Card card)
        {
            _cards.Remove(card);
            View.UpdateCount(_cards.Count);
        }

        public Card TakeFromTop()
        {
            Card card = _cards.Remove(_cards.Count - 1);
            View.UpdateCount(_cards.Count);
            return card;
        }

        public void Shuffle()
        {
            _cards.Shuffle(_shuffleRNG);
        }
    }
}
