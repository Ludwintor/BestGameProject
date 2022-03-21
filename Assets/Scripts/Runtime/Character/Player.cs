using ProjectGame.Actions;
using ProjectGame.Cards;
using UnityEngine;

namespace ProjectGame.Characters
{
    public class Player : Character
    {
        public Hand Hand { get; }

        private readonly TargetingSystem _targetingSystem;
        private int _maxEnergy;
        private int _energy;

        public Player(Hand hand, TargetingSystem targetingSystem)
        {
            Hand = hand;
            _targetingSystem = targetingSystem;
            _maxEnergy = 3;
            _energy = _maxEnergy;
            hand.View.CardLeftHand += OnCardLeftHand;
            hand.View.CardEnterHand += OnCardEnterHand;
            targetingSystem.TargetSelected += OnTargetSelected;
            targetingSystem.TargetingAborted += OnTargetingAborted;
        }

        public bool CanUseCard(Card card)
        {
            if (card.Cost > _energy)
                return false;
            return true;
        }

        public void UseCard(Card card, Character target)
        {
            _energy -= card.Cost;
            card.Use(this, target);
            Hand.Remove(card);
            // Replace with sending to deck
            Object.Destroy(card.View.gameObject);
        }

        private void OnCardLeftHand(Card card)
        {
            if (_targetingSystem.IsTargeting)
                return;
            if (CanUseCard(card))
            {
                _targetingSystem.BeginTargeting(card);
            }
            else
            {
                Hand.View.ReturnCard(card);
            }
        }

        private void OnCardEnterHand(Card card)
        {
            _targetingSystem.StopTargeting();
            Hand.View.ReturnCard(card);
        }

        private void OnTargetSelected(Card card, Character target)
        {
            _targetingSystem.StopTargeting();
            ActionManager actionManager = Game.GetSystem<ActionManager>();
            actionManager.AddToBottom(new UseCardAction(this, target, card));
        }

        private void OnTargetingAborted(Card card)
        {
            Hand.View.ReturnCard(card);
        }
    }
}
