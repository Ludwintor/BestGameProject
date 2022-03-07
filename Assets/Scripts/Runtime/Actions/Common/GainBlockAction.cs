using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Actions
{
    public class GainBlockAction : Action
    {
        private readonly Character _target;
        private readonly int _amount;

        public GainBlockAction(Character target, int amount, float delay) : base(delay)
        {
            _target = target;
            _amount = amount;
        }

        public override void Tick()
        {
            TickDuration();
        }

        protected override void OnDone()
        {
            Debug.Log($"Gained {_amount} block");
        }
    }
}
