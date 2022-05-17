using System;
using ProjectGame.Cards;
using UnityEngine;

namespace ProjectGame
{
    public class CardReward : Reward
    {
        public int chooseCount { get; }
        public Sprite sprite { get; }
        private Card[] _cards;
        private Card _gainedCard;
        public CardReward(Sprite sprite, Card[] cards, int chooseCount)
        {
            _cards = cards;
            this.chooseCount = chooseCount;
            this.sprite = sprite;
            gained = false;
            UpdateDescription();
        }

        public void UpdateDescription()
        {
            description = chooseCount > 1
                ? $"Add {chooseCount} cards to your deck"
                : "Add a card to your deck";
        }

        public override void TryGain(Action<bool> success, RewardManager rewardManager)
        {
            rewardManager.GainCardReward(_cards, (card, picked) =>
            {
                _gainedCard = card;
                if (_gainedCard == null)
                {
                    success(false);
                    
                    InvokeGained(false);
                    return;
                }
                Gain(rewardManager);
                success(true);
            });
        }

        public override void Gain(RewardManager rewardManager)
        {
            gained = true;
            Debug.Log($"Added a card {_gainedCard.Name}");
            //TODO implement gain
            InvokeGained(true);
        }

        public override Sprite GetSprite()
        {
            return sprite;
        }
    }
}