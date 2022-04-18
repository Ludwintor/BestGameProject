using ProjectGame.DungeonMap;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

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
        
        private Vector2[,] _roomPositions;

        private static void ResetPosition(RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
        }

        private static Vector2 RandomOnUnitSphere(Random rng)
        {
            float angle = (float) rng.NextDouble() * 2 * Mathf.PI;
            Vector2 vector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            return vector;
        }

        private static Vector2 RandomVectorInRange(Random rng, float minRadius, float maxRadius)
        {
            Vector2 offset = RandomOnUnitSphere(rng);
            double rngDouble = rng.NextDouble();
            double radius = (rngDouble * (maxRadius - minRadius)) + minRadius;
            return offset * (float)radius;
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

        public void GenerateUI(Map map, Random rng)
        {
            _roomPositions = new Vector2[map.Columns, map.Rows];
            RectTransform mapParent = new GameObject("Map").AddComponent<RectTransform>();
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

                GameObject roomUI = new GameObject();
                RectTransform roomTransform = roomUI.AddComponent<RectTransform>();
                roomTransform.SetParent(roomParent);
                ResetPosition(roomTransform);

                Vector2 offset = RandomVectorInRange(rng, _minRadius, _maxRadius);

                if (room.Position.y == map.Rows - 1)
                {
                    offset = new Vector2(0, _lastOffset);
                }

                roomTransform.anchoredPosition = RoomPosition(room, map.Columns, map.Rows) + offset;
                _roomPositions[room.Position.x, room.Position.y] = roomTransform.anchoredPosition;
                SetSize(roomTransform, room.Data.Size, room.Data.Size);
                
                roomUI.AddComponent<RoomUIElement>().GenerateUI(room, _pathColor);
            }

            float maxX = float.MinValue;
            float minX = float.MaxValue;
            float maxY = float.MinValue;
            float minY = float.MaxValue;
            
            foreach (RoomNode room in map)
            {
                if (!room.HasConnection) continue;
                
                Vector2 roomPos = _roomPositions[room.Position.x, room.Position.y];
                
                float halfSize = room.Data.Size / 2f;
                if (roomPos.x + halfSize > maxX) maxX = roomPos.x + halfSize;
                if (roomPos.x - halfSize < minX) minX = roomPos.x - halfSize;
                if (roomPos.y + halfSize > maxY) maxY = roomPos.y + halfSize;
                if (roomPos.y - halfSize < minY) minY = roomPos.y - halfSize;
                
                foreach (RoomNode childRoom in room.ChildrenNodes)
                {
                    GameObject pathUI = new GameObject();
                    RectTransform pathTransform = pathUI.AddComponent<RectTransform>();
                    Image pathImage = pathUI.AddComponent<Image>();
                    pathTransform.SetParent(pathParent);
                    ResetPosition(pathTransform);
                    
                    Vector2 childRoomPos = _roomPositions[childRoom.Position.x, childRoom.Position.y];
                    Vector2 pathDirection = (childRoomPos - roomPos).normalized;

                    Vector2 pathStartPoint = roomPos + pathDirection * room.Data.Radius;
                    Vector2 pathEndPoint = childRoomPos - pathDirection * childRoom.Data.Radius;

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
            
            mapParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Max(Mathf.Abs(maxX), Mathf.Abs(minX) * 2f));
            mapParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxY - minY);
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
}