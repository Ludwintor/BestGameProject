using ProjectGame.Characters;
using UnityEngine;
using UnityEngine.UI;

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

        private void Start()
        {
            _playerUI.Init(Game.Dungeon.Player);
        }

        public Vector3 ScreenToWorld(Vector2 screenPosition)
        {
            Vector3 position = new Vector3(screenPosition.x, screenPosition.y, _uiCanvas.planeDistance);
            return _uiCanvas.worldCamera.ScreenToWorldPoint(position);
        }
    }
}
