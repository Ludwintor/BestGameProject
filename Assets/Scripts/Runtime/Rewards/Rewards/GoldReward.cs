using System;
using UnityEngine;

namespace ProjectGame
{
    public class GoldReward : Reward
    {
        public int amount { get; }
        public Sprite sprite { get; }
        private string _additionalDescription;
        
        public GoldReward(Sprite sprite, int amount, string additionalDescription = "")
        {
            this.amount = amount;
            this.sprite = sprite;
            gained = false;
            _additionalDescription = additionalDescription;
            UpdateDescription();
        }

        public void UpdateDescription()
        {
            description = $"{amount} Gold";
            if (_additionalDescription.Length > 0)
            {
                description += $" ({_additionalDescription})";
            }
        }

        public override void TryGain(Action<bool> success, RewardManager rewardManager)
        {
            Gain(rewardManager);
            
            success(true);
        }

        public override void Gain(RewardManager rewardManager)
        {
            gained = true;
            Debug.Log($"Gained {amount} gold");
            //TODO implement gain
            InvokeGained(true);
        }

        public override Sprite GetSprite()
        {
            return sprite;
        }
    }
}