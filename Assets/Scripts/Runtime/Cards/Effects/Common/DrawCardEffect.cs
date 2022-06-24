using ProjectGame.Actions;
using ProjectGame.Cards;
using ProjectGame.Characters;
using UnityEngine;

namespace ProjectGame.Effects
{
    [CreateAssetMenu(fileName = "New DrawCardEffect", menuName = "Game/Effects/Common/DrawCard")]
    public class DrawCardEffect : EffectData
    {
        [SerializeField] private int _drawCount;
        [SerializeField] private int _extraCardsPerUpgrade;

        public override void Execute(Card card, Character source, Character target)
        {
            AddToBottom(new DrawCardAction((Player)source, 0.2f, GetMiscValue(card.TimesUpgraded)));
        }

        public override int GetMiscValue(int timesUpgraded)
        {
            return _drawCount + _extraCardsPerUpgrade * timesUpgraded;
        }
    }
}
