using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace ProjectGame
{
    public class MasterSoundSlider : MonoBehaviour
    {
        public AudioMixer masterMixer;
        public float MasterVolume;
        public Slider MasterSlider;
        void Start()
        {
            MasterSlider.onValueChanged.AddListener(ValueChangeCheck);
        }
        public void ValueChangeCheck(float value)
        {
            if(MasterSlider.value == 0)
            {
                MasterVolume = 0;
            } 
            else
            {
                masterMixer.SetFloat("MasterVolume", Mathf.Log(MasterSlider.value) * 20); 
            }
        }
    }
}
