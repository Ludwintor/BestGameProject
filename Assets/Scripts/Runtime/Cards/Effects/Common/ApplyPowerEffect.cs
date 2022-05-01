using ProjectGame.Actions;
using ProjectGame.Cards;
using ProjectGame.Characters;
using ProjectGame.Powers;
using UnityEngine;

namespace ProjectGame.Effects
{
    [CreateAssetMenu(fileName = "New ApplyPowerEffect", menuName = "Game/Effects/Powers/Apply Power")]
    public class ApplyPowerEffect : EffectData
    {
        [SerializeField] private PowerData _powerToApply;
        [SerializeField] private int _basePowerStack;
        [SerializeField] private int _powerStackPerUpgrade;
        [SerializeField] private bool _targetSelf;

        public override void Execute(Card card, Character source, Character target)
        {
            Character finalTarget = _targetSelf ? source : target;
            AddToBottom(new ApplyPowerAction(finalTarget, _powerToApply, GetMiscValue(card.TimesUpgraded)));
        }

        public override int GetMiscValue(int timesUpgraded)
        {
            return _basePowerStack + _powerStackPerUpgrade * timesUpgraded;
        }
    }
}
