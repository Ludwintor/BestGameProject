using ProjectGame.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Actions
{
    public class GainBlockAction : Action
    {
        private readonly Character _target;
        private readonly int _amount;

        public GainBlockAction(Character target, int amount)
        {
            _target = target;
            _amount = amount;
        }

        public override void OnStart()
        {
            _target.GainBlock(_amount);
            Debug.Log($"Gained {_amount} block");
            Done();
        }

        public override void Tick()
        {
        }
    }
}
