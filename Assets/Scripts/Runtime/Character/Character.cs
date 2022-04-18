using ProjectGame.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Characters
{
    public abstract class Character
    {
        public Stats Stats { get; }

        protected ActionManager ActionManager => Game.GetSystem<ActionManager>();

        public Character()
        {
            Stats = new Stats();
        }

        public virtual void TriggerStartTurn(int currentTurn) { }
        public virtual void TriggerEndTurn(int currentTurn) { }

        public virtual void TakeDamage(DamageInfo info)
        {
            Debug.Assert(info.Damage >= 0, "Damage must be non-negative");
            Stats.Health -= info.Damage;
            Stats.Health = Mathf.Clamp(Stats.Health, 0, Stats.MaxHealth);
            Debug.Log($"Character took {info.Damage}");
        }

        public virtual void GainBlock(int block)
        {
            Debug.Assert(block >= 0, "Block gain must be non-negative");
            Stats.Block += block;
        }

        public virtual void LoseBlock(int block = -1)
        {
            Debug.Assert(block >= -1, "Block lose must be non-negative or -1, if losing all block");
            Stats.Block -= block == -1 ? Stats.Block : block;
        }

        public virtual void ChangeGold(int gold)
        {
            Stats.Gold += gold;
        }
    }
}
