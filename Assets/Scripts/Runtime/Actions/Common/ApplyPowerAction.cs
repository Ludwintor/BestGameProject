using System.Collections;
using System.Collections.Generic;
using ProjectGame.Characters;
using ProjectGame.Powers;
using UnityEngine;

namespace ProjectGame.Actions
{
    public class ApplyPowerAction : Action
    {
        private readonly Character _target;
        private readonly PowerData _power;
        private readonly int _stack;

        public ApplyPowerAction(Character target, PowerData power, int stack)
        {
            _target = target;
            _power = power;
            _stack = stack;
        }

        public override void OnStart()
        {
            _target.PowerGroup.Add(_power, _stack);
            Done();
        }

        public override void Tick()
        {
        }
    }
}
