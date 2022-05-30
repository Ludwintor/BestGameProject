using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectGame
{
    public class PopUpBoilingBlood : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] GameObject Description;
        [SerializeField] GameObject Health;
        [SerializeField] GameObject Cash;
        private void Start()
        {
            Description.SetActive(false);
        }

        private void OnMouseOver()
        {
            Cash.SetActive(false);
            Health.SetActive(false);
            Description.SetActive(true);
        }

        private void OnMouseExit()
        {
            Cash.SetActive(true);
            Health.SetActive(true);
            Description.SetActive(false);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
