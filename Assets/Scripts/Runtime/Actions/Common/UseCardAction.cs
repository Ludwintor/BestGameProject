using ProjectGame.Cards;
using ProjectGame.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Actions
{
    public class UseCardAction : Action
    {
        private readonly Player _player;
        private readonly Character _target;
        private readonly Card _card;

        public UseCardAction(Player player, Character target, Card card) : base()
        {
            _player = player;
            _target = target;
            _card = card;
        }

        public override void OnStart()
        {
            // TODO: Очередь карты подошла и она вот-вот будет разыграна.
            // Можно ли её еще разыграть? Может враг уже мёртв или ход был закончен принудительно другими действиями?
            _player.UseCard(_card, _target);
            Done();
        }

        public override void Tick()
        {
        }
    }
}
