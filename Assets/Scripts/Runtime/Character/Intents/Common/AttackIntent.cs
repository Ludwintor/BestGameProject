using ProjectGame.Actions;
using ProjectGame.Characters;
using UnityEngine;

namespace ProjectGame.Intents
{
    [CreateAssetMenu(fileName = "New AttackIntent", menuName = "Game/Intents/Common/Attack", order = 0)]
    public sealed class AttackIntent : IntentData
    {
        [SerializeField] private int _baseDamage;

        public override void Execute(Enemy enemy)
        {
            Player player = Game.Dungeon.Player;
            ActionManager.AddToTop(new DamageAction(player, new DamageInfo(enemy, GetValue(enemy)), 0.2f));
        }

        public override int GetValue(Enemy enemy)
        {
            // TODO: Calculate damage
            return _baseDamage;
        }
    }
}
