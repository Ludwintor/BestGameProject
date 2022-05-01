using System.Collections;
using System.Collections.Generic;
using ProjectGame.Actions;
using UnityEngine;

namespace ProjectGame.Powers
{
    [CreateAssetMenu(fileName = "StrengthPower", menuName = "Game/Powers/StrengthPower")]
    public class StrengthPower : PowerData
    {
        public override int AtDamageInflict(int currentDamage, DamageInfo info, int stack)
        {
            return currentDamage + stack;
        }
    }
}
