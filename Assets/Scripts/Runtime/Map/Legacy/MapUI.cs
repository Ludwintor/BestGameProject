using System;
using System.Collections.Generic;
using ProjectGame.DungeonMap;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class MapUI : MonoBehaviour
    {
        [SerializeField] private float _cellSize = 100f;
        [SerializeField] private float _minRadius = 1f;
        [SerializeField] private float _maxRadius = 10f;
        [SerializeField] private float _lastOffset = 100f;

        [Header("Path Settings")] [SerializeField]
        private Sprite _path;

        [SerializeField] private float _pathSize = 50f;
        [SerializeField] private Color _pathColor = Color.white;

        [SerializeField] private Sprite _background;
        [SerializeField] private Vector2 _mapExtends;
        [SerializeField] private RoomUIElement _roomUIPrefab;

        private MapPlayerTracker _mapPlayerTracker;
        private Vector2[,] _roomOffsets;
        private readonly List<RoomUIElement> _roomUIElements = new List<RoomUIElement>();

        public event Action onMapDirty;
        public RoomNode[] available { get; private set; }


        private static void ResetPosition(RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
        }

        private static Vector2 RandomVectorInRange(RNG rng, float minRadius, float maxRadius)
        {
            Vector2 offset = rng.OnUnitCircle();
            float radius = rng.NextFloat(minRadius, maxRadius);
            return offset * radius;
        }

        private Vector2 RoomPosition(RoomNode room, int xMax, int yMax)
        {
            return new Vector2((room.Position.x + 0.5f - xMax / 2f) * _cellSize,
                (room.Position.y + 0.5f - yMax / 2f) * _cellSize);
        }

        private static void SetSize(RectTransform rectTransform, float xSize, float ySize)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, xSize);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ySize);
        }

        public void GenerateUI(MapPlayerTracker playerTracker, Map map, RNG rng)
        {
            _mapPlayerTracker = playerTracker;
            playerTracker.onPlayerMoved += OnPlayerMoved;
            playerTracker.onMapDirty += OnMapDirty;

            _roomOffsets = new Vector2[map.Columns, map.Rows];
            GameObject mapParentObject = new GameObject("Map");
            RectTransform mapParent = mapParentObject.AddComponent<RectTransform>();
            Image backgroundImage = mapParentObject.AddComponent<Image>();
            backgroundImage.sprite = _background;
            backgroundImage.type = Image.Type.Sliced;
            RectTransform roomParent = new GameObject("Rooms").AddComponent<RectTransform>();
            RectTransform pathParent = new GameObject("Paths").AddComponent<RectTransform>();
            mapParent.SetParent(transform);
            ResetPosition(mapParent);

            pathParent.SetParent(mapParent);
            roomParent.SetParent(mapParent);
            ResetPosition(roomParent);
            ResetPosition(pathParent);

            foreach (RoomNode room in map)
            {
                if (!room.HasConnection) continue;
                Vector2 offset = RandomVectorInRange(rng, _minRadius, _maxRadius);

                if (room.Position.y == map.Rows - 1) offset = new Vector2(0, _lastOffset);
                _roomOffsets[room.Position.x, room.Position.y] = offset;
            }

            float maxX = float.MinValue;
            float minX = float.MaxValue;
            float maxY = float.MinValue;
            float minY = float.MaxValue;

            foreach (RoomNode room in map)
            {
                if (!room.HasConnection) continue;

                RoomUIElement roomUIElement = Instantiate(_roomUIPrefab);
                RectTransform roomTransform = roomUIElement.GetComponent<RectTransform>();
                roomTransform.SetParent(roomParent);
                ResetPosition(roomTransform);

                roomTransform.anchoredPosition = RoomPosition(room, map.Columns, map.Rows) +
                                                 _roomOffsets[room.Position.x, room.Position.y];
                Vector2 roomPos = roomTransform.anchoredPosition;
                
                SetSize(roomTransform, room.Data.Size, room.Data.Size);
                

                // Vector2 roomPos = _roomOffsets[room.Position.x, room.Position.y];

                float halfSize = room.Data.Size / 2f;
                if (roomPos.x + halfSize > maxX) maxX = roomPos.x + halfSize;
                if (roomPos.x - halfSize < minX) minX = roomPos.x - halfSize;
                if (roomPos.y + halfSize > maxY) maxY = roomPos.y + halfSize;
                if (roomPos.y - halfSize < minY) minY = roomPos.y - halfSize;

                foreach (RoomNode parentNode in room.ParentNodes)
                {
                    GameObject pathUI = new GameObject();
                    Path path = new Path
                    {
                        start = parentNode,
                        end = room,
                        gameObject = pathUI
                    };
                    roomUIElement.path.Add(path);
                    
                    RectTransform pathTransform = pathUI.AddComponent<RectTransform>();
                    Image pathImage = pathUI.AddComponent<Image>();
                    pathTransform.SetParent(pathParent);
                    ResetPosition(pathTransform);

                    Vector2 childRoomPos = RoomPosition(parentNode, map.Columns, map.Rows) +
                                           _roomOffsets[parentNode.Position.x, parentNode.Position.y];
                    Vector2 pathDirection = (childRoomPos - roomPos).normalized;

                    Vector2 pathStartPoint = roomPos + pathDirection * room.Data.Radius;
                    Vector2 pathEndPoint = childRoomPos - pathDirection * parentNode.Data.Radius;

                    Vector2 pathVector = pathEndPoint - pathStartPoint;

                    Vector2 center = pathStartPoint + pathVector / 2;

                    pathTransform.anchoredPosition = center;

                    float pathWidth = pathVector.magnitude;
                    pathWidth -= pathWidth % _pathSize;
                    pathTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pathWidth);
                    pathTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _pathSize);

                    Vector2 smthNormalized = pathVector.normalized;

                    float sin = 1 * smthNormalized.y;
                    float cos = 1 * smthNormalized.x;

                    float angle = Mathf.Atan2(sin, cos);
                    pathTransform.rotation = Quaternion.Euler(Vector3.forward * angle * Mathf.Rad2Deg);
                    pathImage.sprite = _path;
                    pathImage.color = _pathColor;
                    pathImage.type = Image.Type.Tiled;
                }
                
                roomUIElement.GenerateUI(room, _pathColor);
                roomUIElement.Subscribe(this);
                roomUIElement.onButtonPressed += OnButtonPressed;
                _roomUIElements.Add(roomUIElement);
            }

            foreach (RectTransform childTransform in pathParent.GetComponentsInChildren<RectTransform>())
            {
                Vector2 anchoredPosition = childTransform.anchoredPosition;
                anchoredPosition.y -= (maxY + minY) / 2;
                childTransform.anchoredPosition = anchoredPosition;
            }

            foreach (RectTransform childTransform in roomParent.GetComponentsInChildren<RectTransform>())
            {
                Vector2 anchoredPosition = childTransform.anchoredPosition;
                anchoredPosition.y -= (maxY + minY) / 2;
                childTransform.anchoredPosition = anchoredPosition;
            }

            mapParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                Mathf.Max(Mathf.Abs(maxX), Mathf.Abs(minX)) * 2f + _mapExtends.x);
            mapParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxY - minY + _mapExtends.y);
            pathParent.anchoredPosition = Vector2.zero;
            pathParent.anchorMin = Vector2.zero;
            pathParent.anchorMax = Vector2.one;
            pathParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                Mathf.Max(Mathf.Abs(maxX), Mathf.Abs(minX) * 2f));
            pathParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxY - minY);

            roomParent.anchoredPosition = Vector2.zero;
            roomParent.anchorMin = Vector2.zero;
            roomParent.anchorMax = Vector2.one;
            roomParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                Mathf.Max(Mathf.Abs(maxX), Mathf.Abs(minX) * 2f));
            roomParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxY - minY);
            available = _mapPlayerTracker.RoomsAvailable();
            onMapDirty?.Invoke();
        }

        private void OnButtonPressed(RoomNode obj)
        {
            int id = -1;
            for (int i = 0; i < available.Length; i++)
            {
                if (available[i] == obj) id = i;
            }

            _mapPlayerTracker.VisitNextRoom(id);
        }

        private void OnMapDirty()
        {
            available = _mapPlayerTracker.RoomsAvailable();
            onMapDirty?.Invoke();
        }

        private void OnPlayerMoved(RoomNode node)
        {
            available = _mapPlayerTracker.RoomsAvailable();
            onMapDirty?.Invoke();
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            _cellSize = Mathf.Max(0, _cellSize);
            _pathSize = Mathf.Max(0, _pathSize);
            _minRadius = Mathf.Max(0, _minRadius);
            _maxRadius = Mathf.Max(_minRadius, _maxRadius);
        }

#endif
    }

    public class Path
    {
        public RoomNode start;
        public RoomNode end;
        public GameObject gameObject;
    }
}