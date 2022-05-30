using System.Collections;
using System.Collections.Generic;
using ProjectGame.Actions;
using ProjectGame.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.Characters
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private DeckView _drawView;
        [SerializeField] private DeckView _masterView;
        [SerializeField] private DeckView _discardView;
        [SerializeField] private HandView _handView;
        [SerializeField] private TargetingSystem _targetingSystem;
        [SerializeField] private TextMeshProUGUI _energyText;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private Image _uiHealthBar;

        private Player _player;

        public void Init(Player player)
        {
            _player = player;
            _drawView.Init(player.DrawDeck);
            _masterView?.Init(player.MasterDeck);
            _discardView.Init(player.DiscardDeck);
            _handView.Init(player.Hand);
            _handView.CardLeftHand += OnCardLeftHand;
            _handView.CardEnterHand += OnCardEnterHand;
            _targetingSystem.TargetSelected += OnTargetSelected;
            _targetingSystem.TargetingAborted += OnTargetingAborted;
            _player.EnergyChanged += UpdateEnergy;
            UpdateEnergy(_player.Energy, _player.MaxEnergy);
            _player.HealthChanged += UpdateHealth;
            UpdateHealth(_player.Health, _player.MaxHealth); 
        }

        private void OnCardLeftHand(CardView cardView)
        {
            if (_targetingSystem.IsTargeting)
                return;
            Card card = cardView.Card;
            if (_player.CanUseCard(card))
            {
                _targetingSystem.BeginTargeting(cardView);
            }
            else
            {
                _handView.ReturnCard(card);
            }
        }

        private void OnCardEnterHand(CardView cardView)
        {
            if (!_targetingSystem.IsTargeting)
                return;
            _targetingSystem.StopTargeting();
            _handView.ReturnCard(cardView.Card);
        }

        private void OnTargetSelected(Card card, Character target)
        {
            _targetingSystem.StopTargeting();
            _player.QueueCard(card, target);
        }

        private void OnTargetingAborted(Card card)
        {
            _handView.ReturnCard(card);
        }

        private void UpdateEnergy(int energy, int maxEnergy)
        {
            _energyText.SetText($"{energy}/{maxEnergy}");
        }

        private void UpdateHealth(int health, int maxHealth)
        {
            float percent = (float)health / maxHealth;
            _uiHealthBar.fillAmount = percent;
            _healthText.SetText($"HP: {health}/{maxHealth}");
        }
    }
}
