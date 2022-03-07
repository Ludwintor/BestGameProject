using ProjectGame.Effects;
using UnityEngine;

namespace ProjectGame.Cards
{
    [CreateAssetMenu(fileName = "New Card Data", menuName = "Game/Cards/Card")]
    public class CardData : ScriptableObject
    {
        private const string DESCRIPTION_TOOLTIP = "For dynamic values use !XY!, where X - 0-based effect index" +
                                                   "\nY - Dynamic value type." +
                                                   "\nAvailable types: A - Damage, B - Defence, M - Misc Value" +
                                                   "\nex: Deal !0A! damage";

        public string Id => _id;
        public string Name => _name;
        public string RawDescription => _rawDescription;
        public Sprite ForegroundImage => _foregroundImage;
        public int BaseCost => _baseCost;
        public int UpgradedCost => _upgradedCost;
        public EffectData[] Effects => _effects;

        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField, Tooltip(DESCRIPTION_TOOLTIP)] private string _rawDescription;
        [SerializeField] private Sprite _foregroundImage;
        [SerializeField] private int _baseCost;
        [SerializeField] private int _upgradedCost;
        [SerializeField] private EffectData[] _effects;
    }
}
