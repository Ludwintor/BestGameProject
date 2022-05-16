using ProjectGame.Characters;
using ProjectGame.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class UIManager : MonoBehaviour, ISystem
    {
        public PlayerUI PlayerUI => _playerUI;
        public MapWindow MapWindow => _mapWindow;

        [SerializeField] private Canvas _uiCanvas;
        [SerializeField] private PlayerUI _playerUI;
        [Header("Map")]
        [SerializeField] private Button _mapButton;
        [SerializeField] private MapWindow _mapWindow;

        private void Awake()
        {
            Game.RegisterSystem(this);
        }

        private void Start()
        {
            _playerUI.Init(Game.Dungeon.Player);
            _mapButton.onClick.AddListener(ShowMap);
        }

        public Vector3 ScreenToWorld(Vector2 screenPosition)
        {
            Vector3 position = new Vector3(screenPosition.x, screenPosition.y, _uiCanvas.planeDistance);
            return _uiCanvas.worldCamera.ScreenToWorldPoint(position);
        }

        private void ShowMap()
        {
            _mapWindow.Show(Game.Dungeon.Map);
        }
    }
}
