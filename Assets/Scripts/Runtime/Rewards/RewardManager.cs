using System;
using System.Collections.Generic;
using ProjectGame.Cards;
using UnityEngine;

namespace ProjectGame
{
    public class RewardManager : MonoBehaviour, ISystem
    {
        [SerializeField] private GameObject _rewardMenuPrefab;
        private RewardMenu _allRewardsSubMenu;
        private CardPickMenu _cardPickSubMenu;
        
        private GameObject _rewardMenu;

        private readonly List<Reward> _rewards = new List<Reward>();
        private bool _menuOpened;
        private void Awake()
        {
            _rewardMenu = Instantiate(_rewardMenuPrefab, transform);
            _allRewardsSubMenu = _rewardMenu.GetComponentInChildren<RewardMenu>(true);
            _cardPickSubMenu = _rewardMenu.GetComponentInChildren<CardPickMenu>(true);
            CloseMenu();
            
            Game.RegisterSystem(this);
        }
        
        private void Start()
        {
            _allRewardsSubMenu.Init(this);
        }

        public void GainAllRewards()
        {
            _allRewardsSubMenu.Reward(_rewards.ToArray());
            OpenMenu();
            GoToAllRewardsMenu();
        }

        public void GainCardReward(IEnumerable<Card> cards, Action<Card, bool> completed)
        {
            if(!_menuOpened) OpenMenu();
            
            GoToCardPickMenu();
            _cardPickSubMenu.PickACard(cards, completed);
        }

        public void DiscardAllRewards()
        {
            _rewards.Clear();
            CloseMenu();
        }
        
        public void AddReward(Reward reward)
        {
            _rewards.Add(reward);
            reward.Gained += GainedReward;
        }

        private void GainedReward(Reward reward, bool gained)
        {
            if (gained)
            {
                reward.Gained -= GainedReward;
                _rewards.Remove(reward);
            }

            if (_rewards.Count == 0)
            {
                CloseMenu();
            }
            else
            {
                GoToAllRewardsMenu();   
            }
        }

        private void GoToAllRewardsMenu()
        {
            _allRewardsSubMenu.gameObject.SetActive(true);
            _cardPickSubMenu.gameObject.SetActive(false);
        }

        private void GoToCardPickMenu()
        {
            _allRewardsSubMenu.gameObject.SetActive(false);
            _cardPickSubMenu.gameObject.SetActive(true);
        }

        private void OpenMenu()
        {
            _menuOpened = true;
            _rewardMenu.SetActive(true);
        }
    
        private void CloseMenu()
        {
            _menuOpened = false;
            _rewardMenu.SetActive(false);
            _allRewardsSubMenu.gameObject.SetActive(false);
            _cardPickSubMenu.gameObject.SetActive(false);
        }
    }
}
