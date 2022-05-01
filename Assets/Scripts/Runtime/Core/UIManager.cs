using ProjectGame.Characters;
using UnityEngine;

namespace ProjectGame
{
    public class UIManager : MonoBehaviour, ISystem
    {
        public PlayerUI PlayerUI => _playerUI;

        [SerializeField] private Canvas _uiCanvas;
        [SerializeField] private PlayerUI _playerUI;

        private void Awake()
        {
            Game.RegisterSystem(this);
        }

        public Vector3 ScreenToWorld(Vector2 screenPosition)
        {
            Vector3 position = new Vector3(screenPosition.x, screenPosition.y, _uiCanvas.planeDistance);
            return _uiCanvas.worldCamera.ScreenToWorldPoint(position);
        }
    }
}
