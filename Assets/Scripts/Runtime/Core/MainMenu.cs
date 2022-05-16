using ProjectGame.Characters;
using ProjectGame.DungeonMap;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectGame
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private PlayerData _selectedPlayer;
        [SerializeField] private MapData _mapData;

        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartClicked);   
        }

        private void OnStartClicked()
        {
            Game.StartGame(_selectedPlayer, _mapData);
        }
    }
}
