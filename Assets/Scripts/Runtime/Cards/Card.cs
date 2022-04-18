using ProjectGame.Characters;
using ProjectGame.Effects;

namespace ProjectGame.Cards
{
    public class Card
    {
        public CardView View { get; set; }
        public string Name => _data.RawName;
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

        public void UpdateVisual(Character target = null)
        {
            View.UpdateName(_data.GetName(_timesUpgraded));
            View.UpdateImage(_data.ForegroundImage);
            View.UpdateCost(Cost.ToString());
            View.UpdateDescription(_data.GetDescription(_timesUpgraded, target));
        }
    }
}
