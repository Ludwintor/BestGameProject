using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class RewardMenu : MonoBehaviour
    {
        [SerializeField] private Transform _buttonsParent;
        [SerializeField] private RewardButton _buttonPrefab;
        [SerializeField] private Button _skipButton; 

        private RewardManager _rewardManager;

        public readonly List<RewardButton> rewardButtons = new List<RewardButton>();
        private int _currentID;

        private void Awake()
        {
            _skipButton.onClick.AddListener(Skip);
        }

        public void Init(RewardManager rewardManager)
        {
            _rewardManager = rewardManager;
        }

        public void Reward(Reward[] rewards)
        {
            if (rewardButtons.Count < rewards.Length)
            {
                for (int i = rewardButtons.Count; i < rewards.Length; i++)
                {
                    rewardButtons.Add(InstantiateButton());
                }
            }
            
            for (int i = 0; i < rewardButtons.Count; i++)
            {
                if (i < rewards.Length)
                {
                    rewardButtons[i].SetReward(rewards[i]);
                    rewardButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    rewardButtons[i].DiscardReward();
                }
            }
        }

        private void Skip()
        {
            foreach (RewardButton rewardButton in rewardButtons)
            {
                rewardButton.DiscardReward();
                rewardButton.SetActive(false);
            }
            _rewardManager.DiscardAllRewards();
        }
        
        private RewardButton InstantiateButton()
        {
            RewardButton rewardButton = Instantiate(_buttonPrefab, _buttonsParent);
            rewardButton.Init(_rewardManager);
            return rewardButton;
        }
    }
}
