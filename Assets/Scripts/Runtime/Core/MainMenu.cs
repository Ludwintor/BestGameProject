using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectGame
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartClicked);   
        }

        private void OnStartClicked()
        {
            // TODO: Create own scene manager to load and unload scenes
            SceneManager.UnloadSceneAsync((int)SceneIndexes.MainMenu);
            SceneManager.LoadSceneAsync((int)SceneIndexes.Game, LoadSceneMode.Additive);
        }
    }
}
