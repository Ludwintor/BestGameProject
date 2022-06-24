using System.Collections.Generic;
using ProjectGame.DungeonMap;
using UnityEngine;

namespace ProjectGame.Windows
{
    public class MapWindow : Window
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private RoomNodeView _nodePrefab;
        [SerializeField] private Line _linePrefab;
        [SerializeField] private float _nodeRadius;
        [SerializeField] private float _bossNodeRadiusFactor;
        [SerializeField] private float _verticalExtent;
        [Header("Alignment")]
        [SerializeField] private float _gapStep;
        [SerializeField] private float _rowGap;
        [SerializeField] private float _lineWidth;
        [SerializeField] private Vector2 _minJitter;
        [SerializeField] private Vector2 _maxJitter;

        private Map _map;
        private Dictionary<Vector2Int, RoomNodeView> _roomViews = new Dictionary<Vector2Int, RoomNodeView>();
        private System.Action<RoomNode> _roomSelectCallback;
        private RNG _rng = new RNG();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }

        public void Show(Map map)
        {
            base.Show();
            if (_map != map)
            {
                _map = map;
                Create();
                UpdatePaths();
            }
        }

        public void AllowSelectNextRoom(RoomNode currentRoom, System.Action<RoomNode> callback, bool onlyNeighbours = true)
        {
            _roomSelectCallback = callback;
            if (currentRoom == null)
                AllowRooms(_map.GetRow(0));
            else if (!onlyNeighbours)
                AllowRooms(_map.GetRow(currentRoom.Position.y + 1));
            else
                AllowRooms(currentRoom.ChildrenNodes);
        }

        private void AllowRooms(IEnumerable<RoomNode> rooms)
        {
            foreach (RoomNode node in rooms)
            {
                if (!node.HasConnection)
                    continue;
                RoomNodeView view = _roomViews[node.Position];
                SetPathAlpha(node, 1f);
                view.SetInteraction(true);
            }
        }

        private void OnRoomSelected(RoomNode node)
        {
            RoomNodeView view = _roomViews[node.Position];
            view.SetVisitedMarker(true);
            node.MarkAsVisited();
            UpdatePaths();
            _roomSelectCallback?.Invoke(node);
        }

        private void Create()
        {
            float bossNodeRadius = _nodeRadius * _bossNodeRadiusFactor;
            float totalHeight = (_map.Rows - 1) * _rowGap + bossNodeRadius + _verticalExtent * 0.5f;
            float startingX = transform.localPosition.x - (_map.Columns - 1) * _gapStep * 0.5f;
            float startingY = transform.localPosition.y - totalHeight;
            Vector2 startingPosition = new Vector2(startingX, startingY);
            _container.sizeDelta = new Vector2(_container.sizeDelta.x, totalHeight + _nodeRadius + _verticalExtent * 0.5f);
            // Render boss room
            RoomNode bossNode = _map.GetNode(_map.Columns / 2, _map.Rows - 1);
            Debug.Assert(bossNode.HasConnection, "Boss node must have at least one connection");
            RenderNode(bossNode, startingPosition + new Vector2(_gapStep, _rowGap) * bossNode.Position, bossNodeRadius * 2f);
            // Render other rooms
            for (int row = _map.Rows - 2; row >= 0; row--)
                foreach (RoomNode node in _map.GetRow(row))
                {
                    if (!node.HasConnection)
                        continue;
                    Vector2 position = startingPosition + new Vector2(_gapStep, _rowGap) * node.Position;
                    Vector2 jitter = _rng.NextVector2(_minJitter, _maxJitter);
                    RoomNodeView nodeView = RenderNode(node, position + jitter, _nodeRadius * 2f);
                    foreach (RoomNode child in node.ChildrenNodes)
                    {
                        RoomNodeView childView = _roomViews[child.Position];
                        Line line = Instantiate(_linePrefab, _container);
                        line.SetAlpha(0.5f);
                        line.transform.SetAsFirstSibling();
                        line.Set(nodeView, childView, _lineWidth);
                        nodeView.AddConnection(line);
                    }
                }
        }

        private void UpdatePaths()
        {
            foreach (RoomNode node in _map)
            {
                if (!node.HasConnection)
                    continue;
                if (node.WasVisited)
                    SetPathAlpha(node, 1f);
                else
                    SetPathAlpha(node, 0.5f);
                RoomNodeView view = _roomViews[node.Position];
                view.SetInteraction(false);
            }
        }

        private void SetPathAlpha(RoomNode node, float alpha)
        {
            RoomNodeView nodeView = _roomViews[node.Position];
            nodeView.SetAlpha(alpha);
            foreach (RoomNode parent in node.ParentNodes)
            {
                if (!parent.WasVisited)
                    continue;
                RoomNodeView parentView = _roomViews[parent.Position];
                Line line = parentView.GetConnectedLine(nodeView);
                line.SetAlpha(alpha);
            }
        }

        private RoomNodeView RenderNode(RoomNode node, Vector2 position, float size)
        {
            RoomNodeView nodeView = Instantiate(_nodePrefab, _container);
            nodeView.Init(node, position, size);
            nodeView.SetAlpha(0.5f);
            nodeView.SetInteraction(false);
            nodeView.SetVisitedMarker(false);
            nodeView.RoomSelected += OnRoomSelected;
            _roomViews.Add(node.Position, nodeView);
            return nodeView;
        }
    }
}
