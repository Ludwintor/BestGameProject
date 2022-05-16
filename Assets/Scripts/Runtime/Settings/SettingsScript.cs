using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class SettingsScript : MonoBehaviour
    {        
        [SerializeField] GameObject StartScreen;
        [SerializeField] GameObject SettingsScreen;
        public void onClick()
        {
            SettingsScreen.SetActive(true);
            StartScreen.SetActive(false);
        }
    }
}
