using UnityEngine;

namespace ProjectGame.Characters
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Hitbox _hitbox;

        public void Init(Enemy enemy)
        {
            _hitbox.Owner = enemy;
        }
    }
}
