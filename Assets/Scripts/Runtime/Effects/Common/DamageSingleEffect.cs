﻿using ProjectGame.Actions;
using ProjectGame.Cards;
using UnityEngine;

namespace ProjectGame.Effects
{
    [CreateAssetMenu(fileName = "New Damage Single Effect", menuName = "Game/Effects/Damage/DamageSingleEffect")]
    public class DamageSingleEffect : EffectData
    {
        [SerializeField] private int _baseDamage;
        [SerializeField] private int _additionalDamagePerUpgrade;

        public override void Execute(Card card, Character source, Character target)
        {
            // TODO: Брать время из каких нибудь глобальный настроек
            AddToBottom(new DamageAction(target, new DamageInfo(source, GetDamage(card.TimesUpgraded)), 0.1f));
        }

        public override int GetDamage(int timesUpgraded)
        {
            return _baseDamage + _additionalDamagePerUpgrade * timesUpgraded;
        }
    }
}