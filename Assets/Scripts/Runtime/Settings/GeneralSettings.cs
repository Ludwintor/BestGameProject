using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{ 
    public class GeneralButton : MonoBehaviour
    {
        [SerializeField] GameObject SoundSettings;
        [SerializeField] GameObject GeneralSettings;
        [SerializeField] GameObject GraphicsSettings;
        //TODO: изменить скрипт основываясь на OnBecameVisible,
        //перед этим разобраться, почему он не работает
        public void onClick()
        {
            SoundSettings.SetActive(true);
            GeneralSettings.SetActive(false);
            GraphicsSettings.SetActive(false);
        }
    }
}
