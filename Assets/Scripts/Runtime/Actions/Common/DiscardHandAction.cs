using ProjectGame.Cards;
using ProjectGame.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Actions
{
    public class DiscardHandAction : Action
    {
        private readonly Player _player;

        public DiscardHandAction(Player player)
        {
            _player = player;
        }

        public override void OnStart()
        {
            IReadOnlyList<Card> cards = _player.Hand.Cards;
            for (int i = cards.Count - 1; i >= 0; i--)
                _player.DiscardCard(cards[i]);
            Done();
        }

        public override void Tick() { }
    }
}
