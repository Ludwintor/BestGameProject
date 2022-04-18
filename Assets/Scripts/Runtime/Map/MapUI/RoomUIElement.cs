using System;
using System.Collections;
using System.Collections.Generic;
using ProjectGame.DungeonMap;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class RoomUIElement : MonoBehaviour
    {
        public RoomNode roomNode { get; private set; }

        private Image _image;
        private Button _button;

        public void GenerateUI(RoomNode room, Color color)
        {
            _image = gameObject.AddComponent<Image>();
            _button = gameObject.AddComponent<Button>();
            
            _image.sprite = room.Data.Sprite;
            _image.color = color;
            roomNode = room;
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            Debug.Log($"{roomNode.Data.name}");
        }
    }
}
