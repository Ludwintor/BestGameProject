using ProjectGame.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class Character
    {
        public void TakeDamage(DamageInfo info)
        {
            Debug.Log($"Damage received: {info.Damage}");
        }
    }
}
