using UnityEngine;

namespace ProjectGame.Characters
{
    public abstract class CharacterData : ScriptableObject
    {
        public int MaxHealth => _maxHealth;
        public int InitialBlock => _initialBlock;

        [SerializeField] private int _maxHealth;
        [SerializeField] private int _initialBlock;
    }
}
