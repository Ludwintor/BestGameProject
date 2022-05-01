using ProjectGame.Actions;
using ProjectGame.Cards;
using ProjectGame.Characters;
using UnityEngine;

namespace ProjectGame.Effects
{
    [CreateAssetMenu(fileName = "New GainBlockEffect", menuName = "Game/Effects/Block/GainBlock")]
    public class GainBlockEffect : EffectData
    {
        [SerializeField] private int _baseBlock;
        [SerializeField] private int _additionalBlockPerUpgrade;
        [SerializeField] private bool _targetSelf;

        public override void Execute(Card card, Character source, Character target)
        {
            AddToBottom(new GainBlockAction(_targetSelf ? source : target, GetBlock(card.TimesUpgraded)));
        }

        public override int GetBlock(int timesUpgraded)
        {
            return _baseBlock + _additionalBlockPerUpgrade * timesUpgraded;
        }
    }
}
