using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    [CreateAssetMenu(fileName = "New IntentType", menuName = "Game/Intents/Intent Type")]
    public class IntentType : ScriptableObject
    {
        public Sprite Sprite => _sprite;
        public bool HasCounter => _hasCounter;

        [SerializeField] private Sprite _sprite;
        [SerializeField] private bool _hasCounter;
    }
}
