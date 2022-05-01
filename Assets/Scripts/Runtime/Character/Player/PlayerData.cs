using ProjectGame.Cards;
using UnityEngine;

namespace ProjectGame.Characters
{
    [CreateAssetMenu(fileName = "New PlayerData", menuName = "Game/Player")]
    public class PlayerData : CharacterData
    {
        public int BaseMaxEnergy => _baseMaxEnergy;
        public CardData[] StartingCards => _startingCards;

        [SerializeField] private int _baseMaxEnergy;
        [SerializeField] private CardData[] _startingCards;
    }
}
