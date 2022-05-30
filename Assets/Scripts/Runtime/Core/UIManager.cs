using System;
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
        public CardsWindow CardsWindow => _cardsWindow;

        [SerializeField] private Canvas _uiCanvas;
        [SerializeField] private PlayerUI _playerUI;
        [SerializeField] private RectTransform _handUI;
        [Header("Map")]
        [SerializeField] private Button _mapButton;
        [SerializeField] private MapWindow _mapWindow;
        [Header("Cards")]
        [SerializeField] private CardsWindow _cardsWindow;

        private void Awake()
        {
            Game.RegisterSystem(this);
        }

        private void Start()
        {
            _playerUI.Init(Game.Dungeon.Player);
            HideHand();
            _mapButton.onClick.AddListener(ShowMap);
        }

        public Vector3 ScreenToWorld(Vector2 screenPosition)
        {
            Vector3 position = new Vector3(screenPosition.x, screenPosition.y, _uiCanvas.planeDistance);
            return _uiCanvas.worldCamera.ScreenToWorldPoint(position);
        }

        public Vector2 WorldToScreen(Vector3 worldPosition)
        {
            return _uiCanvas.worldCamera.WorldToScreenPoint(worldPosition);
        }

        public void ShowHand()
        {
            _handUI.gameObject.SetActive(true);
        }

        public void HideHand()
        {
            _handUI.gameObject.SetActive(false);
        }

        private void ShowMap()
        {
            _mapWindow.Show(Game.Dungeon.Map);
        }
    }
}
