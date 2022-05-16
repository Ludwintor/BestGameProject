using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class StartScript : MonoBehaviour
    {
        [SerializeField] GameObject StartScreen;
        [SerializeField] GameObject SettingsScreen;
        public void onClick()
        {
            StartScreen.SetActive(true);
            SettingsScreen.SetActive(false);
        }
    }
}
