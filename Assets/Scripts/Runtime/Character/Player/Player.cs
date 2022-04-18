using ProjectGame.Actions;
using ProjectGame.Cards;
using System.Collections.Generic;

namespace ProjectGame.Characters
{
    public sealed class Player : Character
    {
        public Hand Hand { get; }
        public Deck MasterDeck { get; }
        public Deck DrawDeck { get; }
        public Deck DiscardDeck { get; }

        private readonly TargetingSystem _targetingSystem;
        private int _maxEnergy;
        private int _energy;

        public Player(TargetingSystem targetingSystem) : base()
        {
            Hand = new Hand();
            MasterDeck = new Deck();
            DrawDeck = new Deck();
            DiscardDeck = new Deck();
            _targetingSystem = targetingSystem;
            _maxEnergy = 3;
            _energy = _maxEnergy;
            targetingSystem.TargetSelected += OnTargetSelected;
            targetingSystem.TargetingAborted += OnTargetingAborted;
        }

        public void SetupViews(HandView handView, DeckView masterView, DeckView drawView, DeckView discardView)
        {
            handView.Init(Hand);
            handView.CardLeftHand += OnCardLeftHand;
            handView.CardEnterHand += OnCardEnterHand;
            masterView?.Init(MasterDeck);
            drawView.Init(DrawDeck);
            discardView.Init(DiscardDeck);
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
            DiscardCard(card);
            Hand.View.ResetCards();
            Hand.View.AlignCards();
        }

        public void DrawCard()
        {
            Card card = DrawDeck.TakeFromTop();
            Hand.Add(card);
            card.View.transform.position = DrawDeck.View.transform.position;
            Hand.View.AlignCards();
        }

        public void DiscardCard(Card card)
        {
            Hand.Remove(card);
            DiscardDeck.Add(card);
            DiscardDeck.View.MoveToDeck(card);
        }

        public override void TriggerStartTurn(int currentTurn)
        {
            int cardsPerTurn = 4;
            ActionManager.AddToBottom(new DrawCardAction(this, 0.2f, cardsPerTurn));
        }

        public override void TriggerEndTurn(int currentTurn)
        {
            IReadOnlyList<Card> cards = Hand.Cards;
            for (int i = cards.Count - 1; i >= 0; i--)
                DiscardCard(cards[i]);
            LoseBlock();
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
            ActionManager.AddToBottom(new UseCardAction(this, target, card));
        }

        private void OnTargetingAborted(Card card)
        {
            Hand.View.ReturnCard(card);
        }
    }
}
