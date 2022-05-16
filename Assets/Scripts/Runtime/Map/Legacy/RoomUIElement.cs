using System;
using System.Collections.Generic;
using ProjectGame.DungeonMap;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class RoomUIElement : MonoBehaviour
    {
        [SerializeField] private GameObject _wasVisitedIndicator;
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        public readonly List<Path> path = new List<Path>();
        public RoomNode roomNode { get; private set; }

        public event Action<RoomNode> onButtonPressed;

        private MapUI _mapUI;

        public void GenerateUI(RoomNode room, Color color)
        {
            _button.interactable = false;
            _image.sprite = room.Data.Sprite;
            _image.color = color;
            foreach (Path path1 in path)
            {
                Color col = path1.gameObject.GetComponent<Image>().color;
                col.a = 0.5f;
                path1.gameObject.GetComponent<Image>().color = col;
            }

            _wasVisitedIndicator.GetComponent<Image>().color = color;
            // room.Data.Size * 2;
            RectTransform rTransform = (RectTransform)_wasVisitedIndicator.transform;
            rTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, room.Data.Size * 2);
            rTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, room.Data.Size * 2);
            _wasVisitedIndicator.SetActive(false);
            roomNode = room;
            _button.onClick.AddListener(OnClick);
        }


        public void Subscribe(MapUI mapUI)
        {
            _mapUI = mapUI;
            mapUI.onMapDirty += onMapDirty;
        }

        private void onMapDirty()
        {
            if (roomNode.WasVisited)
            {
                SetInteractive(false);
                ColorBlock color = _button.colors;
                Color disabledColor = color.disabledColor;
                disabledColor.a = 1f;
                color.disabledColor = disabledColor;
                _button.colors = color;

                foreach (RoomNode parentNode in roomNode.ParentNodes)
                {
                    if (parentNode.WasVisited)
                        foreach (Path path1 in path)
                        {
                            if (path1.start == parentNode)
                            {
                                Color col = path1.gameObject.GetComponent<Image>().color;
                                col.a = 1f;
                                path1.gameObject.GetComponent<Image>().color = col;
                            }
                        }
                }

                _wasVisitedIndicator.SetActive(true);
                _mapUI.onMapDirty -= onMapDirty;
                return;
            }

            SetInteractive(false);

            foreach (Path path1 in path)
            {
                Color col = path1.gameObject.GetComponent<Image>().color;
                col.a = 0.5f;
                path1.gameObject.GetComponent<Image>().color = col;
            }

            foreach (RoomNode availableRoom in _mapUI.available)
            {
                if (roomNode == availableRoom)
                {
                    SetInteractive(true);
                    foreach (RoomNode parentNode in roomNode.ParentNodes)
                    {
                        if (parentNode.WasVisited)
                            foreach (Path path1 in path)
                            {
                                if (path1.start == parentNode)
                                {
                                    Color col = path1.gameObject.GetComponent<Image>().color;
                                    col.a = 1;
                                    path1.gameObject.GetComponent<Image>().color = col;
                                }
                            }
                    }
                }
            }
        }

        public void SetInteractive(bool interactive)
        {
            _button.interactable = interactive;
        }

        private void OnClick()
        {
            onButtonPressed?.Invoke(roomNode);
        }
    }
}