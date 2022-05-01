using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectGame
{
    public class Startup : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.sceneLoaded += Loaded;
            SceneManager.LoadSceneAsync((int)SceneIndexes.MainMenu, LoadSceneMode.Additive);
        }

        private void Loaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.SetActiveScene(scene);
            SceneManager.sceneLoaded -= Loaded;
            Destroy(gameObject);
        }
    }
}
