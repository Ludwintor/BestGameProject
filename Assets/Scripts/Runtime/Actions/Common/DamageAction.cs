using ProjectGame.Characters;
using ProjectGame.Powers;
using UnityEngine;

namespace ProjectGame.Actions
{
    public class DamageAction : Action
    {
        private readonly Character _target;
        private readonly DamageInfo _info;

        public DamageAction(Character target, DamageInfo info, float delay) : base(delay)
        {
            _target = target;
            _info = info;
        }

        public override void Tick()
        {
            TickDuration();
        }

        protected override void OnDone()
        {
            _target.TakeDamage(_info);
        }
    }

    public struct DamageInfo
    {
        public Character Source { get; }
        public int Damage { get; }

        public DamageInfo(Character source, int damage)
        {
            Source = source;
            Damage = damage;
        }

        public int ApplyPowers(Character target)
        {
            int finalDamage = Damage;
            foreach (Power power in Source.PowerGroup)
                finalDamage = power.AtDamageInflict(finalDamage, this);
            foreach (Power power in target.PowerGroup)
                finalDamage = power.AtDamageReceive(finalDamage, this);
            return finalDamage;
        }
    }
}
