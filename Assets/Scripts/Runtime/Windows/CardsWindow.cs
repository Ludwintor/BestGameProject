using System;
using System.Collections;
using System.Collections.Generic;
using ProjectGame.Cards;
using ProjectGame.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.Windows
{
    public class CardsWindow : Window
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private Button _confirmButton;

        private List<CardView> _cardViews = new List<CardView>();
        private List<Card> _selected = new List<Card>();
        private int _howMany;
        private bool _needSelectAll;
        private Action<IEnumerable<Card>> _onSelected;

        private void Start()
        {
            _confirmButton.onClick.AddListener(OnConfirmed);
        }

        /// <summary>
        /// Show cards in a window
        /// </summary>
        public void Show(IEnumerable<Card> cards)
        {
            base.Show();
            CanHide = true;
            foreach (Card card in cards)
            {
                CardView view = Game.CardsPool.Get();
                view.Init(card);
                view.gameObject.SetActive(true);
                view.transform.SetParent(_container, false);
                view.transform.SetAsLastSibling();
                view.SetDragEnabled(false);
                _cardViews.Add(view);
            }
            _confirmButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// Show cards in a window and allow to select
        /// </summary>
        public void ShowSelect(IEnumerable<Card> cards, int howMany, bool needSelectAll, Action<IEnumerable<Card>> onSelected)
        {
            Show(cards);
            CanHide = false;
            _howMany = howMany;
            _needSelectAll = needSelectAll;
            _onSelected = onSelected;
            foreach (CardView view in _cardViews)
            {
                view.PointerUp += OnCardSelected;
            }
            _selected.Clear();
            _confirmButton.gameObject.SetActive(true);
            CheckConfirmAvailability();
        }

        protected override void OnHide()
        {
            foreach (CardView view in _cardViews)
            {
                view.PointerUp -= OnCardSelected;
                Game.CardsPool.Release(view);
            }
            _cardViews.Clear();
            _confirmButton.gameObject.SetActive(false);
        }

        private void CheckConfirmAvailability()
        {
            _confirmButton.interactable = !_needSelectAll || _selected.Count == _howMany;
        }

        private void OnCardSelected(Card card)
        {
            int index = _selected.IndexOf(card);
            if (index == -1)
            {
                if (_howMany == _selected.Count)
                    return;
                _selected.Add(card);
                // TODO: Add outline shader to mark card as selected
            }
            else
            {
                _selected.RemoveAt(index);
            }
            CheckConfirmAvailability();
        }

        private void OnConfirmed()
        {
            CanHide = true;
            Hide();
            _onSelected?.Invoke(_selected);
            _onSelected = null;
        }
    }
}
