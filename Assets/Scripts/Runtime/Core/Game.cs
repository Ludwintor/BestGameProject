using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class Game : MonoBehaviour
    {
        private static Dictionary<Type, ISystem> _systems = new Dictionary<Type, ISystem>();

        /// <summary>
        /// Добавляет систему в реестр. В 99% случаев вызывать всегда в <c>Awake()</c><para>
        /// </summary>
        public static void RegisterSystem(ISystem system)
        {
            Type systemType = system.GetType();
            if (_systems.ContainsKey(systemType))
            {
                Debug.LogError($"[{nameof(Game)}.{nameof(RegisterSystem)}] {systemType.Name} already in system register");
                return;
            }
            _systems.Add(systemType, system);
        }
        
        /// <summary>
        /// Достаёт систему из реестра
        /// </summary>
        public static T GetSystem<T>() where T : ISystem
        {
            return (T)_systems[typeof(T)];
        }
    }
}
