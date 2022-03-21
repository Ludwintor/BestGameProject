using ProjectGame.Effects;

namespace ProjectGame.Cards
{
    public class Card
    {
        public CardView View => _view;
        public string Name => _data.Name;
        public int TimesUpgraded => _timesUpgraded;
        public bool IsUpgraded => _timesUpgraded > 0;
        public bool NeedTarget => _data.NeedTarget;
        public int Cost => IsUpgraded ? _data.UpgradedCost : _data.BaseCost;

        public Character Owner { get; set; }

        private CardData _data;
        private CardView _view;

        private int _timesUpgraded;

        public Card(CardData data, CardView view)
        {
            _data = data;
            _view = view;
            _timesUpgraded = 0;
            _view.Init(this);
            UpdateVisual();
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
            _view.UpdateName(_data.Name);
            _view.UpdateImage(_data.ForegroundImage);
            _view.UpdateCost(_data.BaseCost.ToString());
            _view.UpdateDescription(_data.GetDescription(TimesUpgraded, target));
        }
    }
}
