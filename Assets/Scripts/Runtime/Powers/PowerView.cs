using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectGame.Powers
{
    public class PowerView : MonoBehaviour
    {
        public Power Power => _power;

        [SerializeField] private Image _powerBar;
        [SerializeField] private TextMeshProUGUI _powerText;
        private Power _power;

        public void Init(Power power)
        {
            _power = power;
            _power.PowerChanged += ChangePower;
            ChangePower(_power);
        }
        
        private void ChangePower(Power power)
        {
            _powerBar.sprite = power.Data.Icon;
            _powerText.SetText(power.Stack.ToString());
            Debug.Log("Power Stack:" + power.Stack);
        }
    }
}
