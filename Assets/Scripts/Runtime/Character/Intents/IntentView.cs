using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.Characters
{
    public class IntentView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _counter;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateIntent(Sprite intentImage, string counter)
        {
            _image.sprite = intentImage;
            _counter.SetText(counter);
        }
    }
}
