using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectGame.Cards
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MouseInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, 
        IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<PointerEventData> DragStarted;
        public event Action<PointerEventData> Dragging;
        public event Action<PointerEventData> DragEnded;
        public event Action<PointerEventData> PointerEnter;
        public event Action<PointerEventData> PointerExit;
        public event Action<PointerEventData> PointerDown;
        public event Action<PointerEventData> PointerUp;
        public bool Interactable { get => _canvasGroup.blocksRaycasts; set => _canvasGroup.blocksRaycasts = value; }
        public bool IsDragged => _dragging;

        private bool _dragging;
        private bool _dragEnabled = true;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void StopDrag()
        {
            _dragging = false;
        }

        public void SetDragEnabled(bool value)
        {
            _dragEnabled = value;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_dragEnabled || eventData.button != PointerEventData.InputButton.Left)
                return;
            
            _dragging = true;
            _canvasGroup.blocksRaycasts = false;
            DragStarted?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_dragEnabled)
                return;
            
            if (_dragging)
                Dragging?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_dragEnabled)
                return;
            
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

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            PointerDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            PointerUp?.Invoke(eventData);
        }
    }
}
