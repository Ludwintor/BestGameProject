using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.DungeonMap
{
    public class Line : MonoBehaviour
    {
        public RoomNodeView From { get; private set; }
        public RoomNodeView To { get; private set; }
        private RectTransform _rectTransform;
        private Image _image;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        public void Set(RoomNodeView from, RoomNodeView to, float width)
        {
            From = from;
            To = to;
            Vector2 fromPos = from.RectTransform.localPosition;
            Vector2 toPos = to.RectTransform.localPosition;
            Vector2 direction = (toPos - fromPos).normalized;
            Vector2 start = fromPos + direction * from.Size * 0.5f;
            Vector2 end = toPos - direction * to.Size * 0.5f;
            Vector2 distanceVector = end - start;
            float distance = distanceVector.magnitude;
            distance -= distance % width;
            _rectTransform.localPosition = start + distanceVector * 0.5f;
            _rectTransform.sizeDelta = new Vector2(distance, width);
            float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
            _rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void SetAlpha(float alpha)
        {
            Color color = _image.color;
            color.a = alpha;
            _image.color = color;
        }
    }
}
