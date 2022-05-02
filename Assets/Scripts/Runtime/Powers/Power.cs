using System.Collections;
using System.Collections.Generic;
using ProjectGame.Actions;
using UnityEngine;

namespace ProjectGame.Powers
{
    public class Power
    {
        public event System.Action<Power> PowerChanged;
        public PowerData Data => _data;
        public int Stack => _stack;

        private readonly PowerData _data;
        private int _stack;

        public Power(PowerData data, int stack)
        {
            _data = data;
            _stack = stack;
        }

        public int AtDamageInflict(int currentDamage, DamageInfo info)
        {
            return _data.AtDamageInflict(currentDamage, info, _stack);
        }

        public int AtDamageReceive(int currentDamage, DamageInfo info)
        {
            return _data.AtDamageReceive(currentDamage, info, _stack);
        }

        public void AtTurnStart(int currentTurn)
        {
            _data.AtTurnStart(currentTurn);
        }

        public void AtTurnEnd(int currentTurn)
        {
            _data.AtTurnEnd(currentTurn);
        }

        public void Increase(int stack)
        {
            _stack += stack;
            PowerChanged?.Invoke(this);
        }

        public void Reduce(int stack)
        {
            _stack -= stack;
            PowerChanged?.Invoke(this);
        }
    }

    public abstract class PowerData : ScriptableObject
    {
        public string Id => _id;
        public string Name => _name;
        public Sprite Icon => _icon;

        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;

        public virtual int AtDamageInflict(int currentDamage, DamageInfo info, int stack) { return currentDamage; }
        public virtual int AtDamageReceive(int currentDamage, DamageInfo info, int stack) { return currentDamage; }
        public virtual void AtTurnStart(int currentTurn) { }
        public virtual void AtTurnEnd(int currentTurn) { }
    }
}
