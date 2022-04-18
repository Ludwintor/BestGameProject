using System;
using ProjectGame.DungeonMap;
using UnityEngine;
using Random = System.Random;

namespace ProjectGame
{
    public abstract class BaseSpawnRule : ScriptableObject
    {
        public int RuleExecutionOrder => ruleExecutionOrder;
        public int SpawnChance => spawnChance;
        public bool CanBeOverriden => canBeOverriden;
        
        [Range(0, 100)] [SerializeField] protected int spawnChance = 100;
        [SerializeField] protected bool canBeOverriden;
        [SerializeField] protected int ruleExecutionOrder = 0;

        public abstract bool CanSpawnInCell(Map map, RoomNode room, RoomData data);

        public virtual bool SpawnInCell(Map map, RoomNode room,RoomData data, Random rng)
        {
            return (CanSpawnInCell(map, room, data) && rng.Next(100) <= SpawnChance);
        }
    }
}