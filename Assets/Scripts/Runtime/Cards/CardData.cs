using ProjectGame.Effects;
using UnityEngine;

namespace ProjectGame.Cards
{
    [CreateAssetMenu(fileName = "New Card Data", menuName = "Game/Cards/Card")]
    public class CardData : ScriptableObject
    {
        public string Id => _id;
        public string Name => _name;
        public string RawDescription => _rawDescription;
        public int BaseCost => _baseCost;
        public int UpgradedCost => _upgradedCost;
        public EffectData[] Effects => _effects;

        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _rawDescription;
        [SerializeField] private int _baseCost;
        [SerializeField] private int _upgradedCost;
        [SerializeField] private EffectData[] _effects;
    }
}
