using ProjectGame.Intents;

using UnityEngine;

namespace ProjectGame.Characters
{
    public abstract class EnemyData : ScriptableObject
    {
        public abstract IntentData DetermineIntent(Enemy enemy, Character target, int currentTurn);
    }
}
