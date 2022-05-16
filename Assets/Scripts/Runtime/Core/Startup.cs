using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ProjectGame
{
    public class Startup : MonoBehaviour
    {
        private void Start()
        {
            SceneLoader.LoadScene(SceneIndexes.MainMenu, Loaded);
        }

        private void Loaded()
        {
            Destroy(gameObject);
        }
    }

    public static class SceneLoader
    {
        private static Action _onLoaded;
        private static Scene? _lastLoaded;

        static SceneLoader()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _lastLoaded = null;
        }

        public static void LoadScene(SceneIndexes sceneIndex, Action onLoaded = null)
        {
            _onLoaded = onLoaded;
            if (_lastLoaded != null)
                SceneManager.UnloadSceneAsync(_lastLoaded.Value);
            SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.SetActiveScene(scene);
            _lastLoaded = scene;
            _onLoaded?.Invoke();
        }
    }
}
