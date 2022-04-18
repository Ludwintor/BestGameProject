using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Actions
{
    public class StartTurnAction : Action
    {
        private readonly TurnManager _turnManager;

        public StartTurnAction(TurnManager turnManager)
        {
            _turnManager = turnManager;
        }

        public override void OnStart()
        {
            _turnManager.StartTurn();
            Done();
        }

        public override void Tick() { }
    }
}
