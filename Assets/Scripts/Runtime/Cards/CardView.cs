using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine.Events;

namespace ProjectGame.Cards
{
    public class CardView : MonoBehaviour
    {
        public event Action<Card> DragStart;
        public event Action<Card> Dragging;
        public event Action<Card> DragEnd;
        public event Action<Card, bool> Hovered;

        public bool IsDragged => _dragged == this;
        public bool IsHovered => _hovered == this;
        public TweenCallback OnMoved { get => _moveTween?.onComplete; set => _moveTween.onComplete = value; }
        public bool CanHover { get; set; } = true;
        public bool CanDrag { get; set; } = true;

        private static CardView _dragged;
        private static CardView _hovered;

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Image _image;

        private Card _card;
        private RectTransform _rectTransform;
        private MouseInteraction _interaction;
        private UIManager _uiManager;

        private TweenerCore<Vector3, Vector3, VectorOptions> _moveTween;
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _rotateTween;
        private TweenerCore<Vector3, Vector3, VectorOptions> _scaleTween;

        #region UNITY MESSAGES
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _interaction = GetComponent<MouseInteraction>();
        }

        private void Start()
        {
            _uiManager = Game.GetSystem<UIManager>();
        }

        private void OnEnable()
        {
            _interaction.DragStarted += OnDragStarted;
            _interaction.Dragging += OnDrag;
            _interaction.DragEnded += OnDragEnded;
            _interaction.PointerEnter += OnPointerEnter;
            _interaction.PointerExit += OnPointerExit;
        }

        private void OnDisable()
        {
            _interaction.DragStarted -= OnDragStarted;
            _interaction.Dragging -= OnDrag;
            _interaction.DragEnded -= OnDragEnded;
            _interaction.PointerEnter -= OnPointerEnter;
            _interaction.PointerExit -= OnPointerExit;
            _moveTween.Kill();
            _rotateTween.Kill();
            _scaleTween.Kill();
        }
        #endregion

        public void Init(Card card)
        {
            _card = card;
        }

        public void UpdateName(string name) => _nameText.SetText(name);
        public void UpdateDescription(string description) => _descriptionText.SetText(description);
        public void UpdateCost(string cost) => _costText.SetText(cost);
        public void UpdateImage(Sprite sprite) => _image.sprite = sprite;

        public void Move(Vector2 position)
        {
            // No sense in tweening if positions are equal
            // also avoiding "divison by zero" in recalculation of speed based tween
            if (position.Equals(_rectTransform.localPosition))
            {
                _moveTween?.Pause();
                return;
            }
            float speed = 1000f * UnityEngine.Random.Range(0.7f, 1.3f);
            if (_moveTween.IsActive())
                _moveTween.ChangeEndValue(position, speed, true).Play();
            else
                _moveTween = _rectTransform.DOLocalMove(position, speed).SetSpeedBased(true)
                                           .SetAutoKill(false);
        }

        public void Rotate(float angle)
        {
            Vector3 rotation = new Vector3(0f, 0f, angle);
            if (_rotateTween.IsActive())
                _rotateTween.ChangeEndValue(rotation, true).Play();
            else
                _rotateTween = _rectTransform.DOLocalRotate(rotation, 0.1f).SetAutoKill(false);
        }

        public void Hover()
        {
            if (_scaleTween.IsActive())
                _scaleTween.ChangeEndValue(Vector3.one * 1.5f, true).Play();
            else
                _scaleTween = _rectTransform.DOScale(Vector3.one * 1.5f, 0.2f).SetAutoKill(false);
        }

        public void Unhover()
        {
            if (_scaleTween.IsActive())
                _scaleTween.ChangeEndValue(Vector3.one, true).Play();
            else
                _scaleTween = _rectTransform.DOScale(Vector3.one, 0.2f).SetAutoKill(false);
        }

        public void AllowInteraction()
        {
            CanHover = true;
            CanDrag = true;
        }

        #region Mouse Interaction Events
        private void OnDragStarted(PointerEventData eventData)
        {
            if (!CanDrag)
                return;
            _dragged = this;
            _moveTween.Pause();
            DragStart?.Invoke(_card);
        }

        private void OnDrag(PointerEventData eventData)
        {
            if (!IsDragged)
                return;
            _rectTransform.position = _uiManager.ScreenToWorld(Input.mousePosition);
            Dragging?.Invoke(_card);
        }

        private void OnDragEnded(PointerEventData eventData)
        {
            if (!CanDrag)
                return;
            _dragged = null;
            DragEnd?.Invoke(_card);
        }

        private void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanHover)
                return;
            _hovered = this;
            Hovered?.Invoke(_card, true);
        }

        private void OnPointerExit(PointerEventData eventData)
        {
            if (IsDragged || !IsHovered)
                return;
            _hovered = null;
            Hovered?.Invoke(_card, false);
        }
        #endregion
    }
}
