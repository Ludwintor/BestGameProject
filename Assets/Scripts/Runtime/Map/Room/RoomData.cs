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

        public Sprite Sprite => _sprite;
        public float Size => _size;
        public float Radius => _radius;
        public RoomType roomType => _roomType;
    }
}