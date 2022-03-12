using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{ 
    public class SoundButton : MonoBehaviour
    {
        public GameObject SoundSettings;
        public void onClick()
        {
            SoundSettings.SetActive(true);
            gameObject.SetActive(false);

        }
    }
}
