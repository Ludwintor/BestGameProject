using ProjectGame.Cards;
using ProjectGame.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Actions
{
    public class DrawCardAction : Action
    {
        private readonly Player _player;
        private readonly float _delay;
        private int _count;

        public DrawCardAction(Player player, float delay, int count = 1) : base(delay)
        {
            _player = player;
            _delay = delay;
            _count = count;
        }

        public override void OnStart()
        {
            if (_player.DrawDeck.Count + _player.DiscardDeck.Count == 0)
            {
                Done();
                return;
            }

            if (_count > _player.DrawDeck.Count)
            {
                int leftToDraw = _count - _player.DrawDeck.Count;
                AddToTop(new DrawCardAction(_player, _delay, leftToDraw));
                AddToTop(new ShuffleEmptyDeckAction(_player));
                _count = _player.DrawDeck.Count;
            }
        }

        public override void Tick()
        {
            Duration -= Time.deltaTime;
            if (Duration > 0f)
                return;

            if (_count > 0)
            {
                Duration = _delay;
                _player.DrawCard();
                _count--;
                return;
            }
            Done();
        }
    }
}
