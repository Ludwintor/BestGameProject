using ProjectGame.Characters;
using ProjectGame.DungeonMap;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _versionText;
        [SerializeField] private VersionData _versionData;

        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartClicked);   
        }

        private void Start()
        {
            _versionText.SetText(_versionData.ToString());
        }

        private void OnStartClicked()
        {
            Game.StartGame(_selectedPlayer, _mapData);
        }
    }
}
