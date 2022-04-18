using ProjectGame.Intents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Characters
{
    [CreateAssetMenu(fileName = "New SequenceEnemy", menuName = "Game/Enemies/Sequence Enemy")]
    public sealed class SequenceEnemy : EnemyData
    {
        [SerializeField] private IntentData[] _intents;

        public override IntentData DetermineIntent(Enemy owner, Character target, int currentTurn)
        {
            int selected = (currentTurn - 1) % _intents.Length;
            return _intents[selected];
        }
    }
}
