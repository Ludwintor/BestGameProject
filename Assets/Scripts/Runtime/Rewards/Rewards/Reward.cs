using System;
using UnityEngine;

namespace ProjectGame
{
    public abstract class Reward
    {
        public  bool gained { get; protected set; }
        public string description { get; protected set; }
        public event Action<Reward, bool> Gained;

        /// <summary>
        /// Gain the reward
        /// </summary>
        public abstract void TryGain(Action<bool> success, RewardManager rewardManager);

        public virtual void Gain(RewardManager rewardManager)
        {
        }

        protected void InvokeGained(bool gained)
        {
            Gained?.Invoke(this, gained);
        }

        public abstract Sprite GetSprite();
    }
}