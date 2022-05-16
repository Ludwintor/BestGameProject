using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProjectGame.DungeonMap;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace ProjectGame
{
    //TODO major fucking cleanup
    public class MapPlayerTracker : MonoBehaviour
    {
        [SerializeField] private int _seed;
        [SerializeField] private MapUI _mapUI;
        [SerializeField] private MapConfig _mapConfig;
        
        public event Action<RoomNode> onPlayerMoved;
        public event Action onMapDirty;
        
        private Map _map;
        private RoomNode _currentNode;

        private bool goToAnyRoom = false;

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            Stopwatch timer = Stopwatch.StartNew();
            RNG rng = new RNG(_seed);
            _map = _mapConfig.GenerateMap(rng);
            timer.Stop();
            
            Debug.Log($"Map generation finished in {timer.ElapsedTicks / 100f}ns");
            _mapUI.GenerateUI(this, _map, rng);
        }

        public RoomNode[] RoomsAvailable()
        {
            if (_currentNode == null)
            {
                return _map.GetRow(0);
            }

            if (goToAnyRoom)
            {
                if (_currentNode.Position.y + 1 < _map.Rows)
                {
                    return _map.GetRow(_currentNode.Position.y + 1);
                }
            }

            return _currentNode.ChildrenNodes.ToArray();
        }
        

        public RoomNode GetCurrentRoom()
        {
            return _currentNode;
        }

        public void SetGoAnyRoom()
        {
            goToAnyRoom = true;
            onMapDirty?.Invoke();
        }

        public void VisitNextRoom(int roomID)
        {
            _currentNode = RoomsAvailable()[roomID];
            _currentNode.MarkAsVisited();
            goToAnyRoom = false;
            onPlayerMoved?.Invoke(_currentNode);
        }

        public void VisitAnyRoom(RoomNode roomNode)
        {
            _currentNode = roomNode;
            _currentNode.MarkAsVisited();
            onMapDirty?.Invoke();
            onPlayerMoved?.Invoke(roomNode);
        }
    }
}
