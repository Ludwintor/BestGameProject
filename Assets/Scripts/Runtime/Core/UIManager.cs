using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class UIManager : MonoBehaviour, ISystem
    {
        [SerializeField]
        private Canvas _uiCanvas;

        private void Awake()
        {
            Game.RegisterSystem(this);
        }

        public Vector3 ScreenToWorld(Vector2 screenPosition)
        {
            Vector3 position = new Vector3(screenPosition.x, screenPosition.y, _uiCanvas.planeDistance);
            return _uiCanvas.worldCamera.ScreenToWorldPoint(position);
        }
    }
}
