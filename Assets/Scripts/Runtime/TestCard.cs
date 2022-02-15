using ProjectGame.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class TestCard : MonoBehaviour
    {
        [SerializeField] private CardData _data;
        private Card _card;
        private Character _enemy;

        private void Start()
        {
            _card = new Card(_data);
            _enemy = new Character();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _card.Use(null, _enemy);
            }
        }
    }
}
