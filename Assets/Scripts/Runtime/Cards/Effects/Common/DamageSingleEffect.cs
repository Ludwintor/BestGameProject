using ProjectGame.Actions;
using ProjectGame.Cards;
using ProjectGame.Characters;
using UnityEngine;

namespace ProjectGame.Effects
{
    [CreateAssetMenu(fileName = "New DamageSingleEffect", menuName = "Game/Effects/Damage/DamageSingle")]
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
