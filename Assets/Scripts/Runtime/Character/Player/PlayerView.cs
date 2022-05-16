using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Characters
{
    public class PlayerView : CharacterView
    {
        private void Start()
        {
            Init(Game.Dungeon.Player);
        }

        public void Init(Player player)
        {
            base.Init(player);
        }
    }
}
