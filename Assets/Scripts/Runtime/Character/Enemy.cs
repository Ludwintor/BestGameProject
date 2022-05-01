using ProjectGame.Actions;
using ProjectGame.Intents;
using UnityEngine;

namespace ProjectGame.Characters
{
    public class Enemy : Character
    {
        public event System.Action<IntentData> IntentDetermined;
        public IntentData CurrentIntent => _currentIntent;

        private EnemyData _data;
        private IntentData _currentIntent;

        public Enemy(EnemyData data) : base(data)
        {
            _data = data;
        }

        public override void TriggerStartTurn(int currentTurn)
        {
            _currentIntent = _data.DetermineIntent(this, null, currentTurn);
            IntentDetermined?.Invoke(_currentIntent);
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
