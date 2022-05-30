using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class TooltipSystem : MonoBehaviour, ISystem
    {
        [SerializeField] private Tooltip _tooltip;

        private void Awake()
        {
            Game.RegisterSystem(this);
        }

        public void Show(string content, string header = "")
        {
            _tooltip.Show(content, header);
        }

        public void Hide()
        {
            _tooltip.Hide();
        }
    }
}
