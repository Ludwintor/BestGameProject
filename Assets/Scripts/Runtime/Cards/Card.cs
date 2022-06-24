using ProjectGame.Characters;
using ProjectGame.Effects;
using UnityEngine;

namespace ProjectGame.Cards
{
    public class Card
    {
        public event System.Action<Card> CardChanged;
        public string Name => _data.GetName(_timesUpgraded);
        public string Description => _data.GetDescription(_timesUpgraded, null);
        public Sprite ForegroundImage => _data.ForegroundImage;
        public int TimesUpgraded => _timesUpgraded;
        public bool IsUpgraded => _timesUpgraded > 0;
        public bool NeedTarget => _data.NeedTarget;
        public int Cost => IsUpgraded ? _data.UpgradedCost : _data.BaseCost;

        public Character Owner { get; set; }

        private CardData _data;

        private int _timesUpgraded;

        public Card(CardData data)
        {
            _data = data;
            _timesUpgraded = 0;
        }

        public void Use(Character source, Character target)
        {
            foreach (EffectData effect in _data.Effects)
            {
                effect.Execute(this, source, target);
            }
        }

        public void Upgrade(int timesUpgrade = 1)
        {
            _timesUpgraded += timesUpgrade;
            CardChanged?.Invoke(this);
        }
    }
}
