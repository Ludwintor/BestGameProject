using System.Collections;
using System.Collections.Generic;
using ProjectGame.Characters;
using UnityEngine;

namespace ProjectGame.Actions
{
    public class EnemyTurnAction : Action
    {
        private readonly Enemy _enemy;

        public EnemyTurnAction(Enemy enemy) : base()
        {
            _enemy = enemy;
        }

        public override void OnStart()
        {
            _enemy.TakeTurn();
            Done();
        }

        public override void Tick() { }
    }
}
