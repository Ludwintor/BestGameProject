using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectGame.Cards
{
    public class HandView : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private float _gapBetweenCards;
        [SerializeField] private float _initialSink;
        [SerializeField] private float _sinkPerCard;
        [SerializeField] private float _angleStep;
        [SerializeField] private float _hoverPush;
        [SerializeField, Range(0f, 1f)] private float _pushFactor;
        [SerializeField] private float _hoveredSink;

        private List<CardView> _views = new List<CardView>();
        private Card _dragged;
        private Card _hovered;
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

        public void AddView(CardView cardView)
        {
            _views.Add(cardView);
            cardView.transform.SetParent(_container, false);
            cardView.DragStart += OnCardDragStart;
            cardView.Dragging += OnCardDragging;
            cardView.DragEnd += OnCardDragEnd;
            cardView.Hovered += OnCardHovered;
            AlignCards();
        }

        public void RemoveView(CardView cardView)
        {
            cardView.DragStart -= OnCardDragStart;
            cardView.Dragging -= OnCardDragging;
            cardView.DragEnd -= OnCardDragEnd;
            cardView.Hovered -= OnCardHovered;
            _views.Remove(cardView);
            AlignCards();
        }

        public void AlignCards()
        {
            int count = _views.Count;
            int hoveredIndex = _views.IndexOf(_hovered?.View);
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
                if (cardView != _hovered?.View)
                {
                    int middleOffset = i - middleIndex;
                    middleOffset += count % 2 == 0 && middleOffset < 0 ? 1 : 0;
                    // Now middle offsets are: -1 0 0 1  OR  -1 0 1
                    angle = _angleStep * -middleOffset;
                    position.y = CalculateSink(middleOffset);
                }
                int hoveredOffset = i - hoveredIndex;
                Vector2 push = hoveredIndex != -1 ? new Vector2(CalculateHoverPush(hoveredOffset), 0f) : Vector2.zero;
                cardView.Move(position + push);
                cardView.Rotate(angle);
            }

            _hovered?.View.transform.SetAsLastSibling();
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

        private void OnCardDragStart(Card card)
        {
            _dragged = card;
            foreach (CardView view in _views)
            {
                if (card.View == view)
                    continue;
                view.OnMoved = null;
                view.CanDrag = false;
                view.CanHover = false;
            }
            AlignCards();
        }

        private void OnCardDragging(Card card)
        {

        }

        private void OnCardDragEnd(Card card)
        {
            card.View.OnMoved += card.View.AllowInteraction;
            card.View.CanHover = false;
            card.View.CanDrag = false;
            card.View.Unhover();
            _hovered = null;
            _dragged = null;
            foreach (CardView view in _views)
            {
                if (card.View == view)
                    continue;
                view.CanDrag = true;
                view.CanHover = true;
            }
            AlignCards();
        }

        private void OnCardHovered(Card card, bool isHovered)
        {
            if (isHovered)
            {
                _hovered = card;
                _hovered.View.Hover();
            }
            else if (!card.View.IsDragged)
            {
                _hovered?.View.Unhover();
                _hovered = null;
            }
            AlignCards();
        }

        private void OnHandEnter(PointerEventData eventData)
        {
            if (_dragged == null)
                return;

        }

        private void OnHandExit(PointerEventData eventData)
        {
            if (_dragged == null)
                return;

            // TODO: Передавать карту для проверки всяких параметров, для последующего использования этой карты
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
