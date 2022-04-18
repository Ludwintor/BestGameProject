using ProjectGame.Characters;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace ProjectGame.Intents
{
    public abstract class IntentData : ScriptableObject
    {
        private const string OVERRIDE_SPRITE_DESC = "If this concrete intent has special sprite, use this to override type sprite";

        public Sprite Sprite => _overrideTypeSprite != null ? _overrideTypeSprite : _type.Sprite;
        public bool HasCounter => _type.HasCounter;

        protected ActionManager ActionManager => Game.GetSystem<ActionManager>();

        [SerializeField] private IntentType _type;
        [SerializeField, Description(OVERRIDE_SPRITE_DESC)] private Sprite _overrideTypeSprite;

        /// <summary>
        /// Разыгрывает данное намерение на игрока
        /// </summary>
        public abstract void Execute(Enemy owner);

        public abstract int GetValue(Enemy owner);
    }
}
