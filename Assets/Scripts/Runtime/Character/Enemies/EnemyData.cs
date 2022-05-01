using ProjectGame.Intents;

namespace ProjectGame.Characters
{
    public abstract class EnemyData : CharacterData
    {
        public abstract IntentData DetermineIntent(Enemy enemy, Character target, int currentTurn);
    }
}
