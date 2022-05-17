using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    [RequireComponent(typeof(Button))]
    public class RewardButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _tmpText;
        private Reward _reward;
        private Button _button;

        private RewardManager _rewardManager;
        private GameObject _gameObject;
        
        public void Init(RewardManager rewardManager)
        {
            _rewardManager = rewardManager;
            _button = GetComponent<Button>();
            _gameObject = gameObject;
            _button.onClick.AddListener(GainReward);
            UpdateRewardButton();
        }

        public void SetReward(Reward reward)
        {
            _reward = reward;
            _image.sprite = _reward.GetSprite();
            _tmpText.text = _reward.description;
            // UpdateRewardButton();
        }

        public void DiscardReward()
        {
            _reward = null;
            UpdateRewardButton();
        }

        public void GainReward()
        {
            _reward.TryGain((success) =>
            {
                if (!success) return;

                DiscardReward();
            }, _rewardManager);
        }
        
        public void UpdateRewardButton()
        {
            if (_reward == null)
            {
                SetActive(false);
                return;
            }
            SetActive(true);
        }
        public void SetActive(bool value)
        {
            _gameObject.SetActive(value);
        }

        public bool HasReward()
        {
            return _reward != null;
        }
    }
}
