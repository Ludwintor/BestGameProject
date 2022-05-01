using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.Characters
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private Image _blockBar;
        [SerializeField] private TextMeshProUGUI _blockText;

        public void UpdateHealth(int current, int max)
        {
            float percent = (float)current / max;
            _healthBar.fillAmount = percent;
            _healthText.SetText($"{current}/{max}");
        }

        public void UpdateBlock(int current)
        {
            //_blockBar.gameObject.SetActive(current > 0);
            //_blockText.SetText(current.ToString());
        }
    }
}