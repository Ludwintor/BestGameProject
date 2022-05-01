using UnityEngine;

namespace ProjectGame.Characters
{
    public abstract class CharacterView : MonoBehaviour
    {
        [SerializeField] private Hitbox _hitbox;
        [SerializeField] private HealthView _healthBar;

        protected void Init(Character character)
        {
            _hitbox.Owner = character;
            character.HealthChanged += UpdateHealth;
            character.BlockChanged += UpdateBlock;
            UpdateHealth(character.Health, character.MaxHealth);
            UpdateBlock(character.Block);
        }

        protected void UpdateHealth(int current, int max)
        {
            _healthBar.UpdateHealth(current, max);
        }

        protected void UpdateBlock(int current)
        {
            _healthBar.UpdateBlock(current);
        }

    }
}