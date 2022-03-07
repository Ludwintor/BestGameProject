using ProjectGame.Effects;

namespace ProjectGame.Cards
{
    public class Card
    {
        public CardView View => _view;
        public int TimesUpgraded => _timesUpgraded;
        public bool IsUpgraded => _timesUpgraded > 0;

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
            UpdateDescription(target);
        }

        public void UpdateDescription(Character target)
        {
            // TODO: Добавить вычисление динамических переменных
            // в зависимости от статов держателя карты и наведённой цели
            string description = _data.RawDescription;
            for (int i = 0; i < _data.Effects.Length; i++)
            {
                EffectData effect = _data.Effects[i];
                string key = $"!{i}A!";
                if (description.Contains(key))
                    description = description.Replace(key, effect.GetDamage(_timesUpgraded).ToString());
                key = $"!{i}B!";
                if (description.Contains(key))
                    description = description.Replace(key, effect.GetBlock(_timesUpgraded).ToString());
                key = $"!{i}M!";
                if (description.Contains(key))
                    description = description.Replace(key, effect.GetMiscValue(_timesUpgraded).ToString());
            }
            _view.UpdateDescription(description);
        }
    }
}
