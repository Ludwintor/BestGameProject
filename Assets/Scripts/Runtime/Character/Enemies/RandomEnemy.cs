using ProjectGame.Intents;
using UnityEngine;

namespace ProjectGame.Characters
{
    [CreateAssetMenu(fileName = "New RandomEnemy", menuName = "Game/Enemies/Random Enemy")]
    public sealed class RandomEnemy : EnemyData
    {
        [SerializeField] private IntentData[] _intents;

        public override IntentData DetermineIntent(Enemy owner, Character target, int currentTurn)
        {
            RNG enemyRandom = Game.Dungeon.EnemyRandom;
            int random = enemyRandom.NextInt(_intents.Length);
            return _intents[random];
        }
    }
}
