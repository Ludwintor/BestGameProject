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
            SceneManager.LoadScene((int)SceneIndexes.MainMenu, LoadSceneMode.Additive);
            Destroy(gameObject);
        }
    }
}
