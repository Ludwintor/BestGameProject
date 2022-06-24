using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ProjectGame.Cards
{
    [CreateAssetMenu(fileName = "New CardDatabase", menuName = "Game/Cards/Database")]
    public class CardDatabase : ScriptableObject
    {
        private CardData[] _nonStartingCards;

        [SerializeField] private CardData[] _cards;

        private void Awake()
        {
            _nonStartingCards = _cards.Where(card => !card.StartingCard).ToArray();
        }

        private void OnValidate()
        {
            Awake();
        }

        public Card TakeRandom(RNG rng)
        {
            return new Card(rng.NextElement(_nonStartingCards));
        }

        public IEnumerable<Card> TakeRandom(RNG rng, int count)
        {
            while (count-- > 0)
                yield return TakeRandom(rng);
        }
    }
}
