using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace ProjectGame
{
    public class BGMSlider : MonoBehaviour
    {
    	public AudioMixer masterMixer;
    	public float bgmVolume;
    	public Slider bgmSlider;
        void Start()
        {
        	bgmSlider.onValueChanged.AddListener(ValueChangeCheck);
        }
        public void ValueChangeCheck(float value)
	    {
            if(bgmSlider.value == 0)
            {
                bgmVolume = 0;
            } 
            else
            {
                masterMixer.SetFloat("BGMVolume", Mathf.Log(bgmSlider.value) * 20); 
            }
	    }
    }
}
