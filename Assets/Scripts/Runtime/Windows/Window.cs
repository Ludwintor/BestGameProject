using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Windows
{
    public abstract class Window : MonoBehaviour
    {
        public bool IsOpen => _openedWindow == this;
        protected bool CanHide = true;

        private static Window _openedWindow;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }

        public void Hide()
        {
            if (!CanHide)
                return;
            OnHide();
            gameObject.SetActive(false);
            _openedWindow = null;
        }

        protected virtual void OnHide() { }

        protected void Show()
        {
            if (_openedWindow != null && !_openedWindow.CanHide)
                return;
            if (_openedWindow != null)
                _openedWindow.Hide();
            gameObject.SetActive(true);
            _openedWindow = this;
        }
    }
}
