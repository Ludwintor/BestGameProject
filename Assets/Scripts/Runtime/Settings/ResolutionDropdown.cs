using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class ResolutionDropdown : MonoBehaviour
    {
    	//Resolution[] resolutions = Screen.resolutions;
        public Dropdown ResDropdown;
        public void Dropdown(int value)
        {
        	switch(ResDropdown.value)
        	{
        		case 0:
        			Screen.SetResolution(1920, 1080, true);
        			break;
        		case 1:
        			Screen.SetResolution(1600, 900, true);
        			break;
        		case 2:
        			Screen.SetResolution(1440, 900, true);
        			break;
        		case 3:
        			Screen.SetResolution(1366, 768, true);
        			break;
        		case 4:
        			Screen.SetResolution(1280, 1024, true);
        			break;
        	}
        }
    }
}
