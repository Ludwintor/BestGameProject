using ProjectGame.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class DungeonManager : MonoBehaviour, ISystem
    {
        public Player Player => _player;

        private Player _player;

        private void Awake()
        {
            Game.RegisterSystem(this);
        }

        public void InitPlayer(Player player)
        {
            _player = player;
        }
    }
}
