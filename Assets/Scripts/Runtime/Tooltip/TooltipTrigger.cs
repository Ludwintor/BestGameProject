using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectGame
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ITooltipProvider _tooltipProvider;

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Game.GetSystem<TooltipSystem>().Show();
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ITooltipProvider
    {
        public string GetTooltipHeader();

        public string GetTooltipContent();
    }
}
