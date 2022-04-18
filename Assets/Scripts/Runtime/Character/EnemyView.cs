using ProjectGame.Intents;
using UnityEngine;

namespace ProjectGame.Characters
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Hitbox _hitbox;
        [SerializeField] private IntentView _intentView;

        public void Init(Enemy enemy)
        {
            _hitbox.Owner = enemy;
            enemy.View = this;
        }

        public void ShowIntent(IntentData intent, int counter)
        {
            _intentView.UpdateIntent(intent.Sprite, intent.HasCounter ? counter.ToString() : "");
        }
    }
}
