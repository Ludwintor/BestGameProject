using ProjectGame.Characters;
using ProjectGame.Effects;
using System.Text;
using UnityEngine;

namespace ProjectGame.Cards
{
    [CreateAssetMenu(fileName = "New Card Data", menuName = "Game/Cards/Card")]
    public class CardData : ScriptableObject
    {
        private const string DESCRIPTION_TOOLTIP = "For dynamic values use !XY!, where X - 0-based effect index" +
                                                   "\nY - Dynamic value type." +
                                                   "\nAvailable types: D - Damage, B - Block, M - Misc Value" +
                                                   "\nex: Deal !0A! damage";
        private const string UPGRADED_NAME_ENDING = "+";

        public string Id => _id;
        public string RawName => _name;
        public string RawDescription => _rawDescription;
        public Sprite ForegroundImage => _foregroundImage;
        public int BaseCost => _baseCost;
        public int UpgradedCost => _upgradedCost;
        public bool NeedTarget => _needTarget;
        public EffectData[] Effects => _effects;

        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField, Tooltip(DESCRIPTION_TOOLTIP)] private string _rawDescription;
        [SerializeField] private Sprite _foregroundImage;
        [SerializeField] private int _baseCost;
        [SerializeField] private int _upgradedCost;
        [SerializeField] private bool _needTarget;
        [SerializeField] private EffectData[] _effects;

        public string GetName(int timesUpgraded)
        {
            string name = RawName;
            if (timesUpgraded > 0)
                name += UPGRADED_NAME_ENDING;
            return name;
        }

        public string GetDescription(int timesUpgraded, Character target)
        {
            // TODO: Добавить вычисление динамических переменных
            // в зависимости от статов держателя карты и наведённой цели
            StringBuilder description = new StringBuilder(RawDescription);
            for (int i = 0; i < Effects.Length; i++)
            {
                EffectData effect = Effects[i];
                string key = $"!{i}D!";
                if (RawDescription.Contains(key))
                    description.Replace(key, effect.GetDamage(timesUpgraded).ToString());
                key = $"!{i}B!";
                if (RawDescription.Contains(key))
                    description.Replace(key, effect.GetBlock(timesUpgraded).ToString());
                key = $"!{i}M!";
                if (RawDescription.Contains(key))
                    description.Replace(key, effect.GetMiscValue(timesUpgraded).ToString());
            }
            return description.ToString();
        }
    }
}
