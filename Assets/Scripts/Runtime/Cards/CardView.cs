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

        public bool IsDragged => _interaction.IsDragged;
        public bool CanRegainInteraction { get; set; }
        public bool Interactable { get => _interaction.Interactable; set => _interaction.Interactable = value; }

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
            Interactable = true;
        }

        public void UpdateName(string name) => _nameText.SetText(name);
        public void UpdateDescription(string description) => _descriptionText.SetText(description);
        public void UpdateCost(string cost) => _costText.SetText(cost);
        public void UpdateImage(Sprite sprite) => _image.sprite = sprite;

        public void StopDrag() => _interaction.StopDrag();

        public void Move(Vector2 position, float duration)
        {
            if (_moveTween.IsActive())
                _moveTween.ChangeEndValue(position, duration, true).Play();
            else
                _moveTween = _rectTransform.DOLocalMove(position, duration)
                                           .OnComplete(RegainInteraction).SetAutoKill(false);
        }

        public void Rotate(float angle, float duration)
        {
            Vector3 rotation = new Vector3(0f, 0f, angle);
            if (_rotateTween.IsActive())
                _rotateTween.ChangeEndValue(rotation, duration, true).Play();
            else
                _rotateTween = _rectTransform.DOLocalRotate(rotation, duration).SetAutoKill(false);
        }

        public void Scale(float scale, float duration)
        {
            if (_scaleTween.IsActive())
                _scaleTween.ChangeEndValue(Vector3.one * scale, duration, true).Play();
            else
                _scaleTween = _rectTransform.DOScale(Vector3.one * scale, duration).SetAutoKill(false);
        }

        private void RegainInteraction()
        {
            if (!CanRegainInteraction)
                return;
            Interactable = true;
        }

        #region Mouse Interaction Events
        private void OnDragStarted(PointerEventData eventData)
        {
            _moveTween.Pause();
            DragStart?.Invoke(_card);
        }

        private void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position = _uiManager.ScreenToWorld(Input.mousePosition);
            Dragging?.Invoke(_card);
        }

        private void OnDragEnded(PointerEventData eventData)
        {
            DragEnd?.Invoke(_card);
        }

        private void OnPointerEnter(PointerEventData eventData)
        {
            Hovered?.Invoke(_card, true);
        }

        private void OnPointerExit(PointerEventData eventData)
        {
            Hovered?.Invoke(_card, false);
        }
        #endregion
    }
}
