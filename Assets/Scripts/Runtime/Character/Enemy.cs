using ProjectGame.Actions;
using ProjectGame.Intents;
using UnityEngine;

namespace ProjectGame.Characters
{
    public class Enemy : Character
    {
        public EnemyView View { get; set; }

        private EnemyData _data;
        private IntentData _currentIntent;

        public Enemy(EnemyData data) : base()
        {
            _data = data;
        }

        public override void TriggerStartTurn(int currentTurn)
        {
            _currentIntent = _data.DetermineIntent(this, null, currentTurn);
            View.ShowIntent(_currentIntent, _currentIntent.GetValue(this));
        }

        public override void TriggerEndTurn(int currentTurn)
        {
            ActionManager.AddToBottom(new EnemyTurnAction(this));
        }

        public void TakeTurn()
        {
            Debug.Log("Enemy doing something...");
            _currentIntent.Execute(this);
        }
    }
}
