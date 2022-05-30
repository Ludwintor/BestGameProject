using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame.Characters
{
    public class SpawnPoint : MonoBehaviour
    {
        public Character Occupier { get; set; }
        public bool IsOccupied => Occupier != null;
    }
}
