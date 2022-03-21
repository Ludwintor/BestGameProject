using UnityEngine;

namespace ProjectGame.Cards
{
    public class TargetArrow : MonoBehaviour
    {
        [SerializeField] private RectTransform _headPrefab;
        [SerializeField] private RectTransform _nodePrefab;
        [SerializeField] private int _nodeCount;
        [SerializeField] private Vector2 _firstControlFactor;
        [SerializeField] private Vector2 _secondControlFactor;
        [SerializeField, Range(0f, 1f)] private float _scaleFactor;

        private RectTransform _head;
        private RectTransform[] _nodes;

        public void Show()
        {
            if (_head == null)
            {
                _nodes = new RectTransform[_nodeCount];
                for (int i = 0; i < _nodes.Length; i++)
                    _nodes[i] = Instantiate(_nodePrefab, transform);
                _head = Instantiate(_headPrefab, transform);
            }
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateArrow(Vector2 start, Vector2 end)
        {
            _nodes[0].position = start;
            _head.position = end;
            for (int i = 1; i < _nodes.Length; i++)
            {
                float t = Mathf.Log((float)i / (_nodeCount + 1) + 1f, 2f);
                _nodes[i].position = BezierCurve(t, start, end);
                _nodes[i].rotation = CalculateRotation(_nodes[i], _nodes[i - 1]);
                _nodes[i].localScale = CalculateScale(i);
            }
            _nodes[0].rotation = _nodes[1].rotation;
            _head.rotation = CalculateRotation(_head, _nodes[_nodeCount - 1]);
            _nodes[0].localScale = CalculateScale(0);
        }

        private Quaternion CalculateRotation(RectTransform lhs, RectTransform rhs)
        {
            float angle = Vector2.SignedAngle(Vector2.up, lhs.position - rhs.position);
            return Quaternion.Euler(0f, 0f, angle);
        }

        private Vector3 CalculateScale(int index)
        {
            float scale = 1f + _scaleFactor * index;
            return new Vector3(scale, scale);
        }

        private Vector2 BezierCurve(float t, Vector2 start, Vector2 end)
        {
            Vector2 p0 = start;
            Vector2 p3 = end;
            Vector2 p1 = p0 + (p3 - p0) * _firstControlFactor;
            Vector2 p2 = p0 + (p3 - p0) * _secondControlFactor;
            float u = 1 - t;
            return
                Mathf.Pow(u, 3) * p0 +
                3 * Mathf.Pow(u, 2) * t * p1 +
                3 * Mathf.Pow(t, 2) * u * p2 +
                Mathf.Pow(t, 3) * p3;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_head == null)
                return;
            Gizmos.color = Color.green;
            Vector2 p0 = _nodes[0].position;
            Vector2 p3 = _head.position;
            Vector2 p1 = p0 + (p3 - p0) * _firstControlFactor;
            Vector2 p2 = p0 + (p3 - p0) * _secondControlFactor;
            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.color = Color.magenta;
            for (int i = 1; i < _nodes.Length; i++)
                Gizmos.DrawLine(_nodes[i - 1].position, _nodes[i].position);
            Gizmos.DrawLine(_nodes[_nodes.Length - 1].position, _head.position);
        }
    }
#endif
}
