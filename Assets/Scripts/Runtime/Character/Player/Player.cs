using ProjectGame.Actions;
using ProjectGame.Cards;
using System;
using System.Collections.Generic;

namespace ProjectGame.Characters
{
    public sealed class Player : Character
    {
        public event Action<int, int> EnergyChanged;
        public int MaxEnergy => _maxEnergy;
        public int Energy => _energy;
        public Hand Hand { get; }
        public Deck MasterDeck { get; }
        public Deck DrawDeck { get; }
        public Deck DiscardDeck { get; }

        private int _maxEnergy;
        private int _energy;

        public Player(PlayerData data) : base(data)
        {
            Hand = new Hand();
            MasterDeck = new Deck();
            DrawDeck = new Deck();
            DiscardDeck = new Deck();
            _maxEnergy = data.BaseMaxEnergy;
        }

        public bool CanUseCard(Card card)
        {
            if (card.Cost > _energy)
                return false;
            return true;
        }

        public void UseCard(Card card, Character target)
        {
            SetEnergy(_energy - card.Cost);
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

        public void QueueCard(Card card, Character target)
        {
            ActionManager.AddToBottom(new UseCardAction(this, target, card));
        }

        public override void TriggerStartTurn(int currentTurn)
        {
            SetEnergy(_maxEnergy);
            int cardsPerTurn = 5;
            ActionManager.AddToBottom(new DrawCardAction(this, 0.2f, cardsPerTurn));
            LoseBlock();
        }

        public override void TriggerEndTurn(int currentTurn)
        {
            SetEnergy(0);
            IReadOnlyList<Card> cards = Hand.Cards;
            for (int i = cards.Count - 1; i >= 0; i--)
                DiscardCard(cards[i]);
        }

        private void SetEnergy(int energy)
        {
            _energy = energy;
            EnergyChanged?.Invoke(_energy, _maxEnergy);
        }
    }
}
