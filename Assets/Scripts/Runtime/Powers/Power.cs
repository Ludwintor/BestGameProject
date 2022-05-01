using System.Collections;
using System.Collections.Generic;
using ProjectGame.Actions;
using UnityEngine;

namespace ProjectGame.Powers
{
    public class Power
    {
        private readonly PowerData _data;
        private int _stack;
    }

    public abstract class PowerData : ScriptableObject
    {
        public Sprite Icon => _icon;

        [SerializeField] private Sprite _icon;

        public virtual int AtDamageInfict(DamageInfo info, int stack) { return info.Damage; }
        public virtual int AtDamageReceive(DamageInfo info, int stack) { return info.Damage; }
        public virtual void AtTurnStart(int currentTurn) { }
        public virtual void AtTurnEnd(int currentTurn) { }
    }
}
