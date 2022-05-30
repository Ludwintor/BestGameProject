using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectGame.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectGame.Cards
{
    public class HandView : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private DeckView _drawView;
        [SerializeField] private DeckView _discardView;
        [Header("Card Alignment")]
        [SerializeField] private float _gapBetweenCards;
        [SerializeField] private float _initialSink;
        [SerializeField] private float _sinkPerCard;
        [SerializeField] private float _angleStep;
        [SerializeField] private float _hoverPush;
        [SerializeField, Range(0f, 1f)] private float _pushFactor;
        [SerializeField] private float _hoveredSink;
        [Header("Card Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _minMoveDuration;
        [SerializeField] private float _maxMoveDuration;
        [SerializeField] private float _rotationDuration;
        [SerializeField] private float _hoverScale;
        [SerializeField] private float _scaleDuration;

        public CardView Dragged => _dragged;
        public CardView Hovered => _hovered;
        public event Action<CardView> CardLeftHand;
        public event Action<CardView> CardEnterHand;

        private Hand _hand;
        private List<CardView> _views = new List<CardView>();
        private CardView _dragged;
        private CardView _hovered;
        private RectTransform _rectTransform;
        private MouseInteraction _interaction;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _interaction = GetComponent<MouseInteraction>();
        }

        private void OnEnable()
        {
            _interaction.PointerEnter += OnHandEnter;
            _interaction.PointerExit += OnHandExit;
        }

        private void OnDisable()
        {
            _interaction.PointerEnter -= OnHandEnter;
            _interaction.PointerExit -= OnHandExit;
        }

        public void Init(Hand hand)
        {
            _hand = hand;
            _hand.CardDrawn += AttachView;
            _hand.CardDiscarded += DeattachView;
            _hand.HandCleared += Clear;
        }

        public void ReturnCard(Card card)
        {
            CardView view = GetView(card);
            if (_dragged == view)
            {
                view.StopDrag();
                OnCardDragEnd(card);
            }
        }
        
        public void AlignCards()
        {
            int count = _views.Count;
            int hoveredIndex = _views.IndexOf(_hovered);
            int middleIndex = count / 2; // 0 1 2 3 4     5 ----- 5/2=2 | -2 -1 0 -1 -2          0 1 2 3     4 -------- 4/2=2 | -2 -1 0 -1
            float gapFactor = count > 1 ? 1 - Mathf.Log(count - 1, 150) : 1;
            float finalGap = _gapBetweenCards * gapFactor;
            Vector2 position = (Vector2)_rectTransform.localPosition - new Vector2((count - 1) * finalGap / 2f + finalGap, 0f);
            for (int i = 0; i < _views.Count; i++)
            {
                position.x += finalGap;
                CardView cardView = _views[i];
                cardView.transform.SetSiblingIndex(i);
                if (cardView.IsDragged)
                    continue;

                float angle = 0f;
                position.y = _hoveredSink;
                if (cardView != _hovered)
                {
                    int middleOffset = i - middleIndex;
                    middleOffset += count % 2 == 0 && middleOffset < 0 ? 1 : 0;
                    // Now middle offsets are: -1 0 0 1  OR  -1 0 1
                    angle = _angleStep * -middleOffset;
                    position.y = CalculateSink(middleOffset);
                }
                int hoveredOffset = i - hoveredIndex;
                Vector2 push = hoveredIndex != -1 ? new Vector2(CalculateHoverPush(hoveredOffset), 0f) : Vector2.zero;
                Vector2 newPosition = position + push;
                float distance = Vector2.Distance(cardView.transform.localPosition, newPosition);
                if (distance != 0)
                {
                    float duration = distance / _moveSpeed;
                    duration = Mathf.Clamp(duration, 0.1f, _maxMoveDuration);
                    cardView.Move(newPosition, duration);
                }
                cardView.Rotate(angle, _rotationDuration);
            }

            if (_hovered != null)
                _hovered.transform.SetAsLastSibling();
        }

        public void ResetCards()
        {
            _hovered = null;
            _dragged = null;
            foreach (CardView card in _views)
                card.Interactable = true;
        }

        private void AttachView(Card card)
        {
            CardView cardView = Game.CardsPool.Get();
            cardView.Init(card);
            cardView.gameObject.SetActive(true);
            cardView.transform.SetParent(_container, false);
            cardView.transform.position = _drawView.transform.position;
            cardView.DragStart += OnCardDragStart;
            cardView.Dragging += OnCardDragging;
            cardView.DragEnd += OnCardDragEnd;
            cardView.Hovered += OnCardHovered;
            _views.Add(cardView);
            AlignCards();
        }

        private void DeattachView(Card card)
        {
            CardView cardView = GetView(card);
            cardView.DragStart -= OnCardDragStart;
            cardView.Dragging -= OnCardDragging;
            cardView.DragEnd -= OnCardDragEnd;
            cardView.Hovered -= OnCardHovered;
            if (_hovered == cardView)
            {
                _hovered = null;
                _dragged = null;
            }
            _views.Remove(cardView);
            _discardView.MoveToDeck(cardView);
            ResetCards();
            AlignCards();
        }

        private void Clear()
        {
            foreach (CardView view in _views)
                Game.CardsPool.Release(view);
            _views.Clear();
        }

        private float CalculateSink(int middleOffset)
        {
            middleOffset = -Mathf.Abs(middleOffset);
            return _initialSink + (_sinkPerCard * (int)(middleOffset * 1.7f));
        }

        private float CalculateHoverPush(int hoveredOffset)
        {
            if (hoveredOffset == 0)
                return 0f;
            float push = _hoverPush * Mathf.Pow(_pushFactor, Mathf.Abs(hoveredOffset) - 1);
            return hoveredOffset < 0 ? -push : push;
        }

        private CardView GetView(Card card)
        {
            return _views.First(view => view.Card == card);
        }

        private void OnCardDragStart(Card draggedCard)
        {
            CardView draggedView = GetView(draggedCard);
            _dragged = draggedView;
            foreach (CardView view in _views)
            {
                if (draggedView == view)
                    continue;
                view.CanRegainInteraction = false;
                view.Interactable = false;
            }
            AlignCards();
        }

        private void OnCardDragging(Card card)
        {

        }

        private void OnCardDragEnd(Card card)
        {
            ResetCards();
            CardView view = GetView(card);
            view.CanRegainInteraction = true;
            view.Interactable = false;
            view.Scale(1f, _scaleDuration);
            AlignCards();
        }

        private void OnCardHovered(Card card, bool isHovered)
        {
            CardView view = GetView(card);
            if (isHovered)
            {
                _hovered = view;
                _hovered.Scale(_hoverScale, _scaleDuration);
            }
            else if (!view.IsDragged && _hovered != null)
            {
                _hovered.Scale(1f, _scaleDuration);
                _hovered = null;
            }
            AlignCards();
        }

        private void OnHandEnter(PointerEventData eventData)
        {
            if (_dragged == null)
                return;
            CardEnterHand?.Invoke(_dragged);
        }

        private void OnHandExit(PointerEventData eventData)
        {
            if (_dragged == null)
                return;
            CardLeftHand?.Invoke(_dragged);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying && _rectTransform != null)
                AlignCards();
        }
#endif
    }
}
