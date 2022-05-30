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
            DrawDeck = new Deck();
            DiscardDeck = new Deck();
            Hand = new Hand(DrawDeck, DiscardDeck);
            MasterDeck = new Deck();
            _maxEnergy = data.BaseMaxEnergy;
            foreach (CardData cardData in data.StartingCards)
                MasterDeck.Add(new Card(cardData));
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
        }

        public void DrawCard()
        {
            Hand.Draw();
        }

        public void DiscardCard(Card card)
        {
            Hand.Discard(card);
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

        public override void TriggerCombatStart()
        {
            foreach (Card card in MasterDeck.Cards)
                DrawDeck.Add(card);
            DrawDeck.Shuffle();
        }

        public override void TriggerCombatEnd()
        {
            DiscardDeck.Clear();
            DrawDeck.Clear();
            Hand.Clear();
            PowerGroup.Clear();
        }

        private void SetEnergy(int energy)
        {
            _energy = energy;
            EnergyChanged?.Invoke(_energy, _maxEnergy);
        }
    }
}
