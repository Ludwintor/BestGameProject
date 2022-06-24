using ProjectGame.Characters;
using UnityEngine;

namespace ProjectGame
{
    [CreateAssetMenu(menuName = "Game/Rooms/Room")]
    public class RoomData : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private float _size;
        [SerializeField] private float _radius;
        [SerializeField] private RoomType _roomType;
        [SerializeField] private EnemyPreset[] _enemiesPresets;

        public Sprite Sprite => _sprite;
        public float Size => _size;
        public float Radius => _radius;
        public RoomType RoomType => _roomType;

        public EnemyData[] GetRandomEnemiesPreset(RNG rng)
        {
            return _enemiesPresets[rng.NextInt(0, _enemiesPresets.Length)].Enemies;
        }
    }

    [System.Serializable]
    public class EnemyPreset
    {
        public EnemyData[] Enemies => _enemies;

        [SerializeField] private EnemyData[] _enemies;
    }
}