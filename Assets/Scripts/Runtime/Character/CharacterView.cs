using UnityEngine;
using ProjectGame.Powers;

namespace ProjectGame.Characters
{
    public abstract class CharacterView : MonoBehaviour
    {
        [SerializeField] private Hitbox _hitbox;
        [SerializeField] private HealthView _healthBar;
        [SerializeField] private PowerGroupView _powersView;

        protected void Init(Character character)
        {
            _hitbox.Owner = character;
            character.HealthChanged += UpdateHealth;
            character.BlockChanged += UpdateBlock;
            UpdateHealth(character.Health, character.MaxHealth);
            UpdateBlock(character.Block);
            _powersView.Init(character.PowerGroup);
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