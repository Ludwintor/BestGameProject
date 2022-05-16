using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.DungeonMap
{
    public class RoomNodeView : MonoBehaviour
    {
        public event System.Action<RoomNode> RoomSelected;
        public RectTransform RectTransform => _rectTransform;
        public Vector2 Size => _rectTransform.sizeDelta;

        [SerializeField] private RectTransform _visitedMarker;

        private RoomNode _node;
        private Image _roomImage;
        private RectTransform _rectTransform;
        private Button _button;
        private List<Line> _lineConnections;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _roomImage = GetComponent<Image>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnSelected);
        }

        public void Init(RoomNode node, Vector2 position, float size)
        {
            _node = node;
            _rectTransform.localPosition = position;
            _rectTransform.sizeDelta = new Vector2(size, size);
            _visitedMarker.sizeDelta = new Vector2(size, size) * 2f;
            SetRoomSprite(node.Data.Sprite);
            _lineConnections = new List<Line>(node.ChildrenNodes.Count);
        }

        public Line GetConnectedLine(RoomNodeView child)
        {
            return _lineConnections.FirstOrDefault(line => line.To == child);
        }

        public void SetRoomSprite(Sprite sprite)
        {
            _roomImage.sprite = sprite;
        }

        public void AddConnection(Line line)
        {
            _lineConnections.Add(line);
        }

        public void SetAlpha(float alpha)
        {
            Color color = _roomImage.color;
            color.a = alpha;
            _roomImage.color = color;
        }

        public void SetInteraction(bool active)
        {
            _button.interactable = active;
        }
        public void SetVisitedMarker(bool active)
        {
            _visitedMarker.gameObject.SetActive(active);
        }

        private void OnSelected()
        {
            RoomSelected?.Invoke(_node);
        }

    }
}
