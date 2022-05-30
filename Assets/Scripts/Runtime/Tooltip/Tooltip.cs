using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private TextMeshProUGUI _content;

        private LayoutElement _layoutElement;
        private UIManager _uiManager;

        private void Awake()
        {
            _layoutElement = GetComponent<LayoutElement>();
        }

        private void Start()
        {
            _uiManager = Game.GetSystem<UIManager>();
        }

        private void Update()
        {
            Vector2 position = Input.mousePosition;
            transform.position = _uiManager.ScreenToWorld(position);
        }

        public void Show(string content, string header)
        {
            if (string.IsNullOrEmpty(header))
            {
                _header.gameObject.SetActive(false);
            }
            else
            {
                _header.gameObject.SetActive(true);
                _header.SetText(header);
            }
            _content.SetText(content);
            FitText();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void FitText()
        {
            _layoutElement.enabled = Mathf.Max(_header.preferredWidth, _content.preferredWidth) >= _layoutElement.preferredWidth;
        }

        private void OnValidate()
        {
            _layoutElement = GetComponent<LayoutElement>();
            FitText();
        }
    }
}
