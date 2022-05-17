using System;
using ProjectGame.Cards;
using UnityEngine;

namespace ProjectGame
{
    public class RewardTest : MonoBehaviour
    {
        [SerializeField] private Sprite goldSprite;
        [SerializeField] private Sprite cardSprite;
        [SerializeField] private CardData[] cardsData;
        private RewardManager _rewardManager;

        public void Test()
        {
            _rewardManager = Game.GetSystem<RewardManager>();
            Card[] cards = new Card[cardsData.Length];
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = new Card(cardsData[i]);
            }
            
            _rewardManager.AddReward(new GoldReward(goldSprite, 56));
            _rewardManager.AddReward(new GoldReward(goldSprite, 22, "Stolen back"));
            _rewardManager.AddReward(new CardReward(cardSprite, cards, 1));
            _rewardManager.GainAllRewards();
        }
    }
}