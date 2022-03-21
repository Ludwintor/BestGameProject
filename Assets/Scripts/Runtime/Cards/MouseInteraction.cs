using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectGame.Cards
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MouseInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> DragStarted;
        public event Action<PointerEventData> Dragging;
        public event Action<PointerEventData> DragEnded;
        public event Action<PointerEventData> PointerEnter;
        public event Action<PointerEventData> PointerExit;

        public bool Interactable { get => _canvasGroup.blocksRaycasts; set => _canvasGroup.blocksRaycasts = value; }
        public bool IsDragged => _dragging;

        private bool _dragging;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void StopDrag()
        {
            _dragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragging = true;
            _canvasGroup.blocksRaycasts = false;
            DragStarted?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragging)
                Dragging?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_dragging)
                return;
            _dragging = false;
            _canvasGroup.blocksRaycasts = true;
            DragEnded?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_dragging)
                return;
            PointerExit?.Invoke(eventData);
        }
    }
}
