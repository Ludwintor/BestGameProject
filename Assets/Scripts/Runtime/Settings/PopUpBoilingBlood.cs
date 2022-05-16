using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class PopUpBoilingBlood : MonoBehaviour
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
    }
}
